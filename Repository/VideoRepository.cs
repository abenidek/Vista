using Microsoft.EntityFrameworkCore;
using Vista.Data.AppDbContext;
using Vista.Data.DTOs;
using Vista.Data.Models;
using Vista.Mappers;
using Vista.Repository.Interfaces;
using Xabe.FFmpeg;

namespace Vista.Repository
{
    public class VideoRepository(VistaDbContext _context, IWebHostEnvironment _env) : IVideoRepository
    {
        private readonly VistaDbContext _context = _context;
        private readonly IWebHostEnvironment _env = _env;

        public async Task<List<VideoSummaryDto>> GetAllAsync()
        {
            return await _context.Videos.Include(vid => vid.User).Select(v => v.ToVideoSummaryDto()).ToListAsync();
        }

        public async Task<VideoDetailDto?> GetByIdAsync(Guid id, Guid currentUserId)
        {
            var Video = await _context.Videos.Include(vid => vid.User).Include(v => v.Comments!).ThenInclude(c => c.User).FirstOrDefaultAsync(v => v.VideoId == id);

            if (Video == null)
                return null;

            var VideoDto = Video.ToVideoDetailDto();
            VideoDto.IsLiked = await _context.LikedVideos.AnyAsync(v => v.VideoId == id && v.UserId == currentUserId);
            VideoDto.IsDisliked = await _context.DislikedVideos.AnyAsync(v => v.VideoId == id && v.UserId == currentUserId);
            VideoDto.isUserFollowed = await _context.UserFollowers.AnyAsync(f => f.FollowedUserId == Video.UserId && f.FollowerUserId == currentUserId);

            return VideoDto;
        }

        public async Task<Video?> CreateAsync(CreateVideoDto videoDto)
        {
            if (videoDto is null)
                return null;

            try
            {
                var Video = videoDto.ToVideoModel();

                var videoPath = Path.Combine(_env.WebRootPath, "Videos");
                if (!Directory.Exists(videoPath))
                    Directory.CreateDirectory(videoPath);

                var VideoFileExtension = Path.GetExtension(videoDto.VideoFile.FileName);
                var VideoFilePath = Path.Combine(videoPath, $"{Guid.NewGuid()}{VideoFileExtension}");
                using (var stream = new FileStream(VideoFilePath, FileMode.Create))
                {
                    await videoDto.VideoFile.CopyToAsync(stream);
                }

                Video.VideoUrl = VideoFilePath;

                var ThumbnailPath = Path.Combine(_env.WebRootPath, "Thumbnails");
                if (!Directory.Exists(ThumbnailPath))
                    Directory.CreateDirectory(ThumbnailPath);

                var ThumbnailFileExtension = Path.GetExtension(videoDto.ThumbnailFile.FileName);
                var ThumbnailFilePath = Path.Combine(ThumbnailPath, $"{Guid.NewGuid()}{ThumbnailFileExtension}");
                using (var stream = new FileStream(ThumbnailFilePath, FileMode.Create))
                {
                    await videoDto.ThumbnailFile.CopyToAsync(stream);
                }

                Video.ThumbnailUrl = ThumbnailFilePath;

                var mediaInfo = await FFmpeg.GetMediaInfo(VideoFilePath);
                var duration = mediaInfo.Duration;
                Video.VideoLength = (duration.Hours < 1) ? duration.ToString("mm':'ss") : duration.ToString("hh':'mm':'ss'");

                await _context.Videos.AddAsync(Video);
                await _context.SaveChangesAsync();

                return Video;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Video?> UpdateAsync(Guid id, UpdateVideoDto video)
        {
            var Video = await _context.Videos.FindAsync(id);

            if (Video == null)
                return null;

            try
            {
                if (File.Exists(Video.ThumbnailUrl))
                    File.Delete(Video.ThumbnailUrl);
                    
                var ThumbnailPath = Path.Combine(_env.WebRootPath, "Thumbnails");
                if (!Directory.Exists(ThumbnailPath))
                    Directory.CreateDirectory(ThumbnailPath);

                var ThumbnailFilePath = Path.Combine(ThumbnailPath, video.ThumbnailFile.FileName);
                
                Video.ThumbnailUrl = ThumbnailFilePath;
                Video.VideoName = video.VideoName;
                Video.VideoDescription = video.VideoDescription;
                Video.CategoryId = video.CategoryId;

                using (var stream = new FileStream(ThumbnailFilePath, FileMode.Create))
                {
                    await video.ThumbnailFile.CopyToAsync(stream);
                }

                await _context.SaveChangesAsync();

                return Video;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Video?> DeleteAsync(Guid id)
        {
            var Video = await _context.Videos.FindAsync(id);

            if (Video == null)
                return null;

            try
            {
                if (File.Exists(Video.VideoUrl))
                    File.Delete(Video.VideoUrl);

                if (File.Exists(Video.ThumbnailUrl))
                    File.Delete(Video.ThumbnailUrl);

                _context.Videos.Remove(Video);
                await _context.SaveChangesAsync();

                return Video;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> LikeVideoAsync(Guid videoId, Guid userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var LikedVideo = await _context.LikedVideos.FirstOrDefaultAsync(l => l.VideoId == videoId && l.UserId == userId);
                var Video = await _context.Videos.FirstOrDefaultAsync(v => v.VideoId == videoId);

                if (LikedVideo is null)
                {
                    await _context.LikedVideos.AddAsync(new LikedVideo { VideoId = videoId, UserId = userId });

                    Video!.Likes++;

                    var DislikedVideo = await _context.DislikedVideos.FirstOrDefaultAsync(d => d.VideoId == videoId && d.UserId == userId);
                    if (DislikedVideo is not null)
                    {
                        _context.DislikedVideos.Remove(DislikedVideo);
                        Video!.Dislikes--;
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return "Video Liked";
                }

                _context.LikedVideos.Remove(LikedVideo);

                Video!.Likes--;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Like Retracted";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DislikeVideoAsync(Guid videoId, Guid userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var DislikedVideo = await _context.DislikedVideos.FirstOrDefaultAsync(l => l.VideoId == videoId && l.UserId == userId);
                var Video = await _context.Videos.FirstOrDefaultAsync(v => v.VideoId == videoId);

                if (DislikedVideo is null)
                {
                    await _context.DislikedVideos.AddAsync(new DislikedVideo { VideoId = videoId, UserId = userId });

                    Video!.Dislikes++;

                    var LikedVideo = await _context.LikedVideos.FirstOrDefaultAsync(d => d.VideoId == videoId && d.UserId == userId);
                    if (LikedVideo is not null)
                    {
                        _context.LikedVideos.Remove(LikedVideo);
                        Video!.Likes--;
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return "Video Disliked";
                }

                _context.DislikedVideos.Remove(DislikedVideo);

                Video!.Dislikes--;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Dislike Retracted";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<VideoSummaryDto>?> GetTrendingVideosAsync()
        {
            var currentDate = DateTime.Now;
            var Videos = await _context.Videos
                .Include(v => v.User)
                .Include(v => v.Comments)
                .Where(v => (currentDate - v.UploadDate).TotalDays <= 7)
                .OrderByDescending(v => v.Likes)
                .ThenByDescending(v => v.UploadDate)
                .ThenByDescending(v => v.Comments!.Count)
                .Take(4)
                .ToListAsync();
            
            return Videos.Select(v => v.ToVideoSummaryDto()).ToList();
        }

        public async Task<List<VideoSummaryDto>?> GetRecommendedVideosAsync(Guid userId)
        {
            var Followings = await _context.UserFollowers.Where(f => f.FollowerUserId == userId).Select(f => f.FollowedUserId).ToListAsync();

            var WatchedCategories = await _context.WatchedVideos
                .Include(w => w.Video)
                .Where(w => w.UserId == userId)
                .Select(w => w.Video!.CategoryId)
                .Distinct()
                .ToListAsync();

            var Videos = await _context.Videos
                .Include(v => v.User)
                .Where(v => WatchedCategories.Contains(v.CategoryId) || Followings.Contains(v.UserId))
                .OrderByDescending(v => v.Likes)
                .ThenByDescending(v => v.UploadDate)
                .ThenByDescending(v => v.Comments!.Count)
                .Take(8)
                .ToListAsync();

            return Videos.Select(v => v.ToVideoSummaryDto()).ToList();
        }
    }
}
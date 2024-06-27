using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            if (!currentUserId.ToString().IsNullOrEmpty())
            {
                VideoDto.IsLiked = await _context.LikedVideos.AnyAsync(v => v.VideoId == id && v.UserId == currentUserId);
                VideoDto.IsDisliked = await _context.DislikedVideos.AnyAsync(v => v.VideoId == id && v.UserId == currentUserId);
                VideoDto.User!.isUserFollowed = await _context.UserFollowers.AnyAsync(f => f.FollowedUserId == Video.UserId && f.FollowerUserId == currentUserId);
            }

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
                var VideoFileName = $"{Guid.NewGuid()}{VideoFileExtension}";
                var VideoFilePath = Path.Combine(videoPath, VideoFileName);
                using (var stream = new FileStream(VideoFilePath, FileMode.Create))
                {
                    await videoDto.VideoFile.CopyToAsync(stream);
                }

                Video.VideoUrl = Path.Combine("http://localhost:5252", "Videos", VideoFileName);

                var ThumbnailPath = Path.Combine(_env.WebRootPath, "Thumbnails");
                if (!Directory.Exists(ThumbnailPath))
                    Directory.CreateDirectory(ThumbnailPath);

                var ThumbnailFileExtension = Path.GetExtension(videoDto.ThumbnailFile.FileName);
                var ThumbnailFileName = $"{Guid.NewGuid()}{ThumbnailFileExtension}";
                var ThumbnailFilePath = Path.Combine(ThumbnailPath, ThumbnailFileName);
                using (var stream = new FileStream(ThumbnailFilePath, FileMode.Create))
                {
                    await videoDto.ThumbnailFile.CopyToAsync(stream);
                }

                Video.ThumbnailUrl = Path.Combine("http://localhost:5252", "Thumbnails", ThumbnailFileName);

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
                var ThumbnailFilePath = Path.Combine(_env.WebRootPath, Video.ThumbnailUrl.TrimStart("http://localhost:5252/".ToCharArray()));
                if (File.Exists(ThumbnailFilePath))
                    File.Delete(ThumbnailFilePath);

                var ThumbnailPath = Path.Combine(_env.WebRootPath, "Thumbnails");
                if (!Directory.Exists(ThumbnailPath))
                    Directory.CreateDirectory(ThumbnailPath);

                var ThumbnailFileExtension = Path.GetExtension(video.ThumbnailFile.FileName);
                var ThumbnailFileName = $"{Guid.NewGuid()}{ThumbnailFileExtension}";
                var ThumbnailFilePathNew = Path.Combine(ThumbnailPath, ThumbnailFileName);
                using (var stream = new FileStream(ThumbnailFilePathNew, FileMode.Create))
                {
                    await video.ThumbnailFile.CopyToAsync(stream);
                }

                Video.ThumbnailUrl = Path.Combine("http://localhost:5252", "Thumbnails", ThumbnailFileName);
                Video.VideoName = video.VideoName;
                Video.VideoDescription = video.VideoDescription;
                Video.CategoryId = video.CategoryId;

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
                var VideoFilePath = Path.Combine(_env.WebRootPath, Video.VideoUrl.TrimStart("http://localhost:5252/".ToCharArray()));
                if (File.Exists(VideoFilePath))
                    File.Delete(VideoFilePath);

                var ThumbnailFilePath = Path.Combine(_env.WebRootPath, Video.ThumbnailUrl.TrimStart("http://localhost:5252/".ToCharArray()));
                if (File.Exists(ThumbnailFilePath))
                    File.Delete(ThumbnailFilePath);

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

        public async Task WatchVideoAsync(Guid videoId, Guid userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var WatchedVideo = await _context.WatchedVideos.FirstOrDefaultAsync(wv => wv.VideoId == videoId && wv.UserId == userId);
                var Video = await _context.Videos.FirstOrDefaultAsync(v => v.VideoId == videoId);

                if (WatchedVideo is null)
                {
                    await _context.WatchedVideos.AddAsync(new WatchedVideo { VideoId = videoId, UserId = userId });

                    Video!.Views++;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<VideoSummaryDto>?> GetWatchedVideoAsync(Guid userId)
        {
            var WatchedVideos = await _context.WatchedVideos.Where(wv => wv.UserId == userId).Select(wv => wv.VideoId).ToListAsync();

            return await _context.Videos.Where(v => WatchedVideos.Contains(v.VideoId)).Select(v => v.ToVideoSummaryDto()).ToListAsync();
        }

        public async Task<string> SaveVideoAsync(Guid videoId, Guid userId)
        {
            var SavedVideo = await _context.SavedVideos.FirstOrDefaultAsync(sv => sv.VideoId == videoId && sv.UserId == userId);

            if (SavedVideo is null)
            {
                await _context.SavedVideos.AddAsync(new SavedVideo { VideoId = videoId, UserId = userId });
                await _context.SaveChangesAsync();

                return "Video Saved";
            }

            _context.SavedVideos.Remove(SavedVideo);
            await _context.SaveChangesAsync();

            return "Video Removed from Saved";
        }

        public async Task<List<VideoSummaryDto>?> GetSavedVideoAsync(Guid userId)
        {
            var SavedVideos = await _context.SavedVideos.Where(sv => sv.UserId == userId).Select(sv => sv.VideoId).ToListAsync();

            return await _context.Videos.Where(v => SavedVideos.Contains(v.VideoId)).Select(v => v.ToVideoSummaryDto()).ToListAsync();
        }

        public async Task<List<VideoSummaryDto>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Videos.Where(v => v.CategoryId == categoryId).Select(v => v.ToVideoSummaryDto()).ToListAsync();
        }
    }
}
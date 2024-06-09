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
            return await _context.Videos.Include(vid => vid.User).Select(vid => vid.ToVideoSummaryDto()).ToListAsync();
        }

        public async Task<VideoDetailDto?> GetByIdAsync(Guid id)
        {
            var Video = await _context.Videos.Include(vid => vid.User).Include(v => v.Comments!).ThenInclude(c => c.User).FirstOrDefaultAsync(v => v.VideoId == id);

            if (Video == null)
                return null;

            return Video.ToVideoDetailDto();
        }

        public async Task<Video?> CreateAsync(CreateVideoDto videoDto)
        {
            if (videoDto is null)
                return null;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var Video = videoDto.ToVideoModel();

                var videoPath = Path.Combine(_env.WebRootPath, "Videos");
                if (!Directory.Exists(videoPath))
                    Directory.CreateDirectory(videoPath);

                var VideoFilePath = Path.Combine(videoPath, videoDto.VideoFile.FileName);
                using (var stream = new FileStream(VideoFilePath, FileMode.Create))
                {
                    await videoDto.VideoFile.CopyToAsync(stream);
                }

                Video.VideoUrl = VideoFilePath;

                var ThumbnailPath = Path.Combine(_env.WebRootPath, "Thumbnails");
                if (!Directory.Exists(ThumbnailPath))
                    Directory.CreateDirectory(ThumbnailPath);

                var ThumbnailFilePath = Path.Combine(ThumbnailPath, videoDto.ThumbnailFile.FileName);
                using (var stream = new FileStream(ThumbnailFilePath, FileMode.Create))
                {
                    await videoDto.ThumbnailFile.CopyToAsync(stream);
                }

                Video.ThumbnailUrl = ThumbnailFilePath;

                var mediaInfo = await FFmpeg.GetMediaInfo(VideoFilePath);
                var duration = mediaInfo.Duration;
                Video.VideoLength = duration.ToString("mm':'ss");

                await _context.Videos.AddAsync(Video);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Video;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<Video?> UpdateAsync(Guid id, UpdateVideoDto video)
        {
            var Video = await _context.Videos.FindAsync(id);

            if (Video == null)
                return null;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (File.Exists(Video.ThumbnailUrl))
                    File.Delete(Video.ThumbnailUrl);

                var ThumbnailPath = Path.Combine(_env.WebRootPath, "Thumbnails");
                if (!Directory.Exists(ThumbnailPath))
                    Directory.CreateDirectory(ThumbnailPath);

                var ThumbnailFilePath = Path.Combine(ThumbnailPath, video.ThumbnailFile.FileName);
                using (var stream = new FileStream(ThumbnailFilePath, FileMode.Create))
                {
                    await video.ThumbnailFile.CopyToAsync(stream);
                }

                Video.ThumbnailUrl = ThumbnailFilePath;
                Video.VideoName = video.VideoName;
                Video.VideoDescription = video.VideoDescription;
                Video.CategoryId = video.CategoryId;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Video;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<Video?> DeleteAsync(Guid id)
        {
            var Video = await _context.Videos.FindAsync(id);

            if (Video == null)
                return null;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (File.Exists(Video.VideoUrl))
                    File.Delete(Video.VideoUrl);

                if (File.Exists(Video.ThumbnailUrl))
                    File.Delete(Video.ThumbnailUrl);

                _context.Videos.Remove(Video);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Video;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Vista.Data.AppDbContext;
using Vista.Data.DTOs;
using Vista.Data.Models;
using Vista.Repository.Interfaces;

namespace Vista.Repository
{
    public class VideoRepository(VistaDbContext _context) : IVideoRepository
    {
        private readonly VistaDbContext _context = _context;

        public async Task<List<VideoSummaryDto>> GetAllAsync()
        {
            return await _context.Videos.Include(vid => vid.User).Select(vid => vid.ToVideoSummaryDto()).ToListAsync();
        }

        public async Task<VideoDetailDto?> GetByIdAsync(Guid id)
        {
            var Video = await _context.Videos.Include(v => v.Comments!).ThenInclude(c => c.User).FirstOrDefaultAsync(v => v.VideoId == id);

            if (Video == null)
                return null;

            return Video.ToVideoDetailDto();
        }

        public async Task<Video> CreateAsync(CreateVideoDto videoDto)
        {
            var Video = videoDto.ToVideoModel();
            await _context.Videos.AddAsync(Video);
            await _context.SaveChangesAsync();
            return Video;
        }

        public async Task<Video?> UpdateAsync(Guid id, UpdateVideoDto video)
        {
            var Video = await _context.Videos.FindAsync(id);

            if (Video == null)
                return null;

            Video.VideoName = video.VideoName;
            Video.VideoDescription = video.VideoDescription;
            Video.CategoryId = video.CategoryId;
            Video.ThumbnailUrl = video.ThumbnailUrl;

            await _context.SaveChangesAsync();

            return Video;
        }

        public async Task<Video?> DeleteAsync(Guid id)
        {
            var Video = await _context.Videos.FindAsync(id);

            if (Video == null)
                return null;

            _context.Videos.Remove(Video);
            await _context.SaveChangesAsync();
            return Video;
        }
    }
}
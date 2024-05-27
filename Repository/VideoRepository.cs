using Microsoft.EntityFrameworkCore;
using Vista.Data.AppDbContext;
using Vista.Data.DTOs;
using Vista.Data.Models;
using Vista.Repository.Interfaces;

namespace Vista.Repository
{
    public class VideoRepository(VistaDbContext _context) : IVideoRepository
    {
        public async Task<List<VideoSummaryDto>> GetAllAsync()
        {
            return await _context.Videos.Select(vid => vid.ToVideoSummaryDto()).ToListAsync();
        }

        public async Task<VideoDetailDto?> GetByIdAsync(Guid id)
        {
            var Video = await _context.Videos.FindAsync(id);

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
using Vista.Data.DTOs;
using Vista.Data.Models;

namespace Vista.Repository.Interfaces;

public interface IVideoRepository
{
    Task<List<VideoSummaryDto>> GetAllAsync();
    Task<VideoDetailDto?> GetByIdAsync(Guid id);
    Task<Video> CreateAsync(CreateVideoDto video);
    Task<Video?> DeleteAsync(Guid id);
}

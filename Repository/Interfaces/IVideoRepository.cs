using Vista.Data.DTOs;
using Vista.Data.Models;

namespace Vista.Repository.Interfaces;

public interface IVideoRepository
{
    Task<List<VideoSummaryDto>> GetAllAsync();
    Task<VideoDetailDto?> GetByIdAsync(Guid id, Guid currentUserId);
    Task<Video?> CreateAsync(CreateVideoDto video);
    Task<Video?> UpdateAsync(Guid id, UpdateVideoDto video);
    Task<Video?> DeleteAsync(Guid id);
    Task<string> LikeVideoAsync(Guid videoId, Guid userId);
    Task<string> DislikeVideoAsync(Guid videoId, Guid userId);
    Task<List<VideoSummaryDto>?> GetTrendingVideosAsync();
    Task<List<VideoSummaryDto>?> GetRecommendedVideosAsync(Guid userId);
    Task WatchVideoAsync(Guid videoId, Guid userId);
    Task<List<VideoSummaryDto>?> GetWatchedVideoAsync(Guid userId);
    Task<string> SaveVideoAsync(Guid videoId, Guid userId);
    Task<List<VideoSummaryDto>?> GetSavedVideoAsync(Guid userId);
    Task<List<VideoSummaryDto>> GetByCategoryAsync(int categoryId);
}

using Vista.Data.DTOs;

namespace Vista;

public interface ISearchRepository
{
    Task<List<VideoSummaryDto>?> SearchVideoAsync(string value);
    Task<List<UserDto>?> SearchUserAsync(string value);
}

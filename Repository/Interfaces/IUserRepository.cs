using Vista.Data.DTOs;
using Vista.Data.Models;

namespace Vista;

public interface IUserRepository
{
    Task<List<UserDto>> GetAllAsync();
    Task<User?> CreateAsync(UserFromGoogleDto userDto);
    Task<ProfileDto?> GetProfile(Guid UserId, Guid currentUserId);
    Task<List<UserDto>?> GetFollowers(Guid current_user);
    Task<List<UserDto>?> GetFollowing(Guid current_user);
    Task<string> FollowUser(Guid UserId, Guid currentUserId);
}

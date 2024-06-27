using Vista.Data.DTOs;
using Vista.Data.Models;

namespace Vista;

public interface IUserRepository
{
    Task<List<UserDto>> GetAllAsync();
    Task<User?> CreateAsync(UserFromGoogleDto userDto);
    Task<ProfileDto?> GetProfileAsync(Guid UserId, Guid currentUserId);
    Task<MyProfileDto?> GetMyProfileAsync(Guid UserId);
    Task<List<UserDto>?> GetFollowersAsync(Guid currentUserId);
    Task<List<UserDto>?> GetFollowingAsync(Guid currentUserId);
    Task<string> FollowUserAsync(Guid UserId, Guid currentUserId);
}

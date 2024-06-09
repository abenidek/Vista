using Vista.Data.DTOs;
using Vista.Data.Models;

namespace Vista;

public interface IUserRepository
{
    Task<List<UserDto>> GetAllAsync();
    Task<User?> CreateAsync(UserFromGoogleDto userDto);
}

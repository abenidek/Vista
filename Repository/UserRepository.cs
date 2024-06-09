using Microsoft.EntityFrameworkCore;
using Vista.Data.AppDbContext;
using Vista.Data.DTOs;
using Vista.Data.Models;
using Vista.Mappers;

namespace Vista;

public class UserRepository(VistaDbContext _context, IWebHostEnvironment _env) : IUserRepository
{
    private readonly VistaDbContext _context = _context;
    private readonly IWebHostEnvironment _env = _env;

    public async Task<List<UserDto>> GetAllAsync()
    {
        return await _context.Users.Select(us => us.ToUserDto()).ToListAsync();
    }

    public async Task<User?> CreateAsync(UserFromGoogleDto userDto)
    {
        if (userDto is null)
            return null;

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var User = userDto.ToUserModel();

            var ProfilePicPath = Path.Combine(_env.WebRootPath, "ProfilePics");
            if (!Directory.Exists(ProfilePicPath))
                Directory.CreateDirectory(ProfilePicPath);

            var ProfilePicFilePath = Path.Combine(ProfilePicPath, userDto.ProfilePicFile.FileName);
            using (var stream = new FileStream(ProfilePicFilePath, FileMode.Create))
            {
                await userDto.ProfilePicFile.CopyToAsync(stream);
            }

            User.ProfilePicUrl = ProfilePicFilePath;

            await _context.Users.AddAsync(User);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return User;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }
}

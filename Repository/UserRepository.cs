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

    public async Task<ProfileDto?> GetProfileAsync(Guid userId, Guid currentUserId)
    {
        var User = await _context.Users.Include(u => u.Videos!).FirstOrDefaultAsync(u => u.UserId == userId);

        if (User is null)
            return null;

        var UserProfileDto = User?.ToProfileDto();
        UserProfileDto!.IsBeingFollowed = await _context.UserFollowers.AnyAsync(f => f.FollowedUserId == userId && f.FollowerUserId == currentUserId);
        return UserProfileDto;
    }

    public async Task<List<UserDto>?> GetFollowersAsync(Guid userId)
    {
        var Followers = await _context.UserFollowers.Where(f => f.FollowedUserId == userId).Include(f => f.FollowerUser).Select(f => f.FollowerUser).ToListAsync();

        if (Followers is null)
            return null;

        var FollowerUsersDto = Followers.Select(f => f?.ToUserDto()).ToList();
        return FollowerUsersDto!;
    }

    public async Task<List<UserDto>?> GetFollowingAsync(Guid userId)
    {
        var Followings = await _context.UserFollowers.Where(f => f.FollowerUserId == userId).Include(f => f.FollowedUser).Select(f => f.FollowedUser).ToListAsync();

        if (Followings is null)
            return null;

        var FollowingUsersDto = Followings.Select(f => f?.ToUserDto()).ToList();
        return FollowingUsersDto!;
    }

    public async Task<string> FollowUserAsync(Guid userId, Guid currentUserId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var User = await _context.UserFollowers.FirstOrDefaultAsync(f => f.FollowedUserId == userId && f.FollowerUserId == currentUserId);
            var FollowedUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            var FollowerUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == currentUserId);

            if (User is null)
            {
                await _context.UserFollowers.AddAsync(new UserFollower { FollowedUserId = userId, FollowerUserId = currentUserId });

                FollowedUser!.FollowersCount++;
                FollowerUser!.FollowingCount++;

                await _context.SaveChangesAsync();
                return "User Followed";
            }

            _context.UserFollowers.Remove(User);

            FollowedUser!.FollowersCount--;
            FollowerUser!.FollowingCount--;

            await _context.SaveChangesAsync();
            return "User Unfollowed";
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }
}

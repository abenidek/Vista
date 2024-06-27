using Microsoft.EntityFrameworkCore;
using Vista.Data.AppDbContext;
using Vista.Data.DTOs;
using Vista.Data.Models;
using Vista.Mappers;

namespace Vista;

public class UserRepository(VistaDbContext _context) : IUserRepository
{
    private readonly VistaDbContext _context = _context;

    public async Task<List<UserDto>> GetAllAsync()
    {
        return await _context.Users.Select(us => us.ToUserDto()).ToListAsync();
    }

    public async Task<User?> CreateAsync(UserFromGoogleDto userDto)
    {
        if (userDto is null)
            return null;
        
        var User = userDto.ToUserModel();

        var UserExist = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

        if(UserExist is null)
        {
            await _context.Users.AddAsync(User);
            await _context.SaveChangesAsync();
            return User;
        }

        return UserExist;
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

    public async Task<MyProfileDto?> GetMyProfileAsync(Guid userId)
    {
        var User = await _context.Users.Include(u => u.Videos!).FirstOrDefaultAsync(u => u.UserId == userId);

        if (User is null)
            return null;

        int totalViews = 0;
        int totalLikes = 0;
        foreach(Video video in User!.Videos!)
        {
            totalViews += video.Views;
            totalLikes += video.Likes;
        }

        var UserDto = User?.ToMyProfileDto();
        UserDto!.TotalViews = totalViews;
        UserDto!.TotalLikes = totalLikes;
        
        return UserDto;
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
                await transaction.CommitAsync();

                return "User Followed";
            }

            _context.UserFollowers.Remove(User);

            FollowedUser!.FollowersCount--;
            FollowerUser!.FollowingCount--;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return "User Unfollowed";
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }
}

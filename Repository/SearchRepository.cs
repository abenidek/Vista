using Vista.Data.DTOs;
using Vista.Data.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Vista.Mappers;

namespace Vista;

public class SearchRepository(VistaDbContext _context) : ISearchRepository
{
    private readonly VistaDbContext _context = _context;
    public async Task<List<VideoSummaryDto>?> SearchVideoAsync(string value)
    {
        var Videos = await _context.Videos.Include(v => v.User).Where(v => EF.Functions.ILike(v.VideoName, $"%{value}%")).ToListAsync();

            return Videos.Select(v => v.ToVideoSummaryDto()).ToList();
    }

    public async Task<List<UserDto>?> SearchUserAsync(string value)
    {
        var Users = await _context.Users.Where(u =>
            EF.Functions.ILike(u.UserName, $"%{value}%")||
            EF.Functions.ILike(u.Name, $"%{value}%")).ToListAsync();

        return Users.Select(u => u.ToUserDto()).ToList();
    }
}

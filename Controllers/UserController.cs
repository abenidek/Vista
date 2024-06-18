using Microsoft.AspNetCore.Mvc;
using Vista.Data.DTOs;
using Vista.Mappers;
using Vista.Repository.Interfaces;

namespace Vista.Controllers;

[Route("/user")]
[ApiController]
public class UserController(IUserRepository _userRepo) : ControllerBase
{
    public readonly IUserRepository _userRepo = _userRepo;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var users = await _userRepo.GetAllAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] UserFromGoogleDto userDto)
    {
        var User = await _userRepo.CreateAsync(userDto);

        if (User is null)
            return BadRequest();

        return Ok(User.ToUserDto());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfileAsync([FromRoute] Guid id, Guid currentUserId)
    {
        var User = await _userRepo.GetProfileAsync(id, currentUserId);

        if (User is null)
            return NotFound();
        
        return Ok(User);
    }

    [HttpGet("follower/{id}")]
    public async Task<IActionResult> GetFollowersAsync([FromRoute] Guid id)
    {
        var Followers = await _userRepo.GetFollowersAsync(id);

        if(Followers is null)
            return NotFound();
        
        return Ok(Followers);
    }

    [HttpGet("following/{id}")]
    public async Task<IActionResult> GetFollowingAsync([FromRoute] Guid id)
    {
        var Followings = await _userRepo.GetFollowingAsync(id);

        if(Followings is null)
            return NotFound();
        
        return Ok(Followings);
    }

    [HttpPut]
    [Route("follow/{id}")]
    public async Task<IActionResult> FollowUserAsync([FromRoute] Guid id, Guid currentUserId)
    {
        var Response = await _userRepo.FollowUserAsync(id, currentUserId);

        return Ok(Response);
    }
}
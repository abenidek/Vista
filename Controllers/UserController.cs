using Microsoft.AspNetCore.Mvc;
using Vista.Data.DTOs;
using Vista.Repository.Interfaces;

namespace Vista.Controllers;

[Route("api/users")]
[ApiController]
public class UserController(IUserRepository _userRepo) : ControllerBase
{

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

        return Ok(User);
    }


}
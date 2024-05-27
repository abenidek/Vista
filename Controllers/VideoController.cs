using Microsoft.AspNetCore.Mvc;
using Vista.Data.DTOs;
using Vista.Data.Models;
using Vista.Repository.Interfaces;

namespace Vista.Controllers;

[Route("api/video")]
[ApiController]
public class VideoController(IVideoRepository _videoRepo) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var Videos = await _videoRepo.GetAllAsync();
        return Ok(Videos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var Video = await _videoRepo.GetByIdAsync(id);

        if (Video == null)
            return NotFound();

        return Ok(Video);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateVideoDto videoDto)
    {
        await _videoRepo.CreateAsync(videoDto);

        var Video = videoDto.ToVideoModel();
        return CreatedAtAction(nameof(GetByIdAsync), new { id = Video.VideoId }, Video.ToVideoDetailDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var Video = await _videoRepo.DeleteAsync(id);

        if(Video is null)
            return NotFound();

        return NoContent();
    }
}

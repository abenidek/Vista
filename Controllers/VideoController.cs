using Microsoft.AspNetCore.Mvc;
using Vista.Data.DTOs;
using Vista.Data.Models;
using Vista.Mappers;
using Vista.Repository.Interfaces;

namespace Vista.Controllers;

[Route("/video")]
[ApiController]
public class VideoController(IVideoRepository _videoRepo) : ControllerBase
{
    public readonly IVideoRepository _videoRepo = _videoRepo;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var Videos = await _videoRepo.GetAllAsync();

        return Ok(Videos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, Guid currentUserId)
    {
        var Video = await _videoRepo.GetByIdAsync(id, currentUserId);

        if (Video == null)
            return NotFound();

        return Ok(Video);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] CreateVideoDto videoDto)
    {
        if (videoDto is null)
            return BadRequest();

        var Video = await _videoRepo.CreateAsync(videoDto);

        return Ok(Video);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync([FromForm] UpdateVideoDto videoDto, Guid id)
    {
        var Video = await _videoRepo.UpdateAsync(id, videoDto);

        if(Video is null)
            return NotFound();

        return NoContent();
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

    [HttpPut]
    [Route("like/{id}")]
    public async Task<IActionResult> LikeAsync([FromRoute] Guid id, Guid userId)
    {
        var Response = await _videoRepo.LikeVideoAsync(id, userId);

        return Ok(Response);
    }

    [HttpPut]
    [Route("dislike/{id}")]
    public async Task<IActionResult> DislikeAsync([FromRoute] Guid id, Guid userId)
    {
        var Response = await _videoRepo.DislikeVideoAsync(id, userId);

        return Ok(Response);
    }

    [HttpGet]
    [Route("trending/")]
    public async Task<IActionResult> GetTrendingAsync()
    {
        var Videos = await _videoRepo.GetTrendingVideosAsync();

        if(Videos is null)
            return NoContent();
        
        return Ok(Videos);
    }

    [HttpGet]
    [Route("recommended/{id}")]
    public async Task<IActionResult> GetRecommendedAsync([FromRoute] Guid id)
    {
        var Videos = await _videoRepo.GetRecommendedVideosAsync(id);

        if(Videos is null)
            return NoContent();
        
        return Ok(Videos);
    }

    [HttpPut]
    [Route("watch/{id}")]
    public async Task<IActionResult> WatchAsync([FromRoute] Guid id, Guid userId)
    {
        await _videoRepo.WatchVideoAsync(id, userId);

        return Ok();
    }

    [HttpGet]
    [Route("history/{id}")]
    public async Task<IActionResult> GetWatchedAsync([FromRoute] Guid id)
    {
        var Videos = await _videoRepo.GetWatchedVideoAsync(id);

        if(Videos is null)
            return NoContent();
        
        return Ok(Videos);
    }

    [HttpPut]
    [Route("save/{id}")]
    public async Task<IActionResult> SaveAsync([FromRoute] Guid id, Guid userId)
    {
        await _videoRepo.SaveVideoAsync(id, userId);

        return Ok();
    }

    [HttpGet]
    [Route("saved/{id}")]
    public async Task<IActionResult> GetSavedAsync([FromRoute] Guid id)
    {
        var Videos = await _videoRepo.GetSavedVideoAsync(id);

        if(Videos is null)
            return NoContent();
        
        return Ok(Videos);
    }

    [HttpGet]
    [Route("category/{id}")]
    public async Task<IActionResult> GetByCategoryAsync([FromRoute] int id)
    {
        var Videos = await _videoRepo.GetByCategoryAsync(id);

        if(Videos is null)
            return NoContent();
        
        return Ok(Videos);
    }
}

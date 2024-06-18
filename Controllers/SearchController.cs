using Microsoft.AspNetCore.Mvc;

namespace Vista;

[Route("/search")]
[ApiController]
public class SearchController(ISearchRepository _searchRepo) : ControllerBase
{
    private readonly ISearchRepository _searchRepo = _searchRepo;

    [HttpGet]
    [Route("video/{value}")]
    public async Task<IActionResult> SearchVideoAsync([FromRoute]string value)
    {
        var Videos = await _searchRepo.SearchVideoAsync(value);

        if(Videos is null)
            return NoContent();
        
        return Ok(Videos);
    }

    [HttpGet]
    [Route("user/{value}")]
    public async Task<IActionResult> SearchUserAsync([FromRoute]string value)
    {
        var Users = await _searchRepo.SearchUserAsync(value);

        if(Users is null)
            return NoContent();
        
        return Ok(Users);
    }
}

using Microsoft.AspNetCore.Mvc;
using Vista.Data.Models;
using Vista.Mappers;

namespace Vista;

[Route("/comment")]
[ApiController]
public class CommentController(ICommentRepository _commentRepo) : ControllerBase
{
    private readonly ICommentRepository _commentRepo = _commentRepo;
    

    [HttpPost("{userId}")]
    public async Task<IActionResult> CreateAsync([FromRoute]Guid userId, CreateCommentDto commentDto)
    {
        var Comment = commentDto.ToCommentModel(userId);

        await _commentRepo.CreateAsync(Comment);

        return Ok(Comment.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute]Guid id, [FromBody]string updatedComment)
    {
        var Comment = await _commentRepo.UpdateAsync(id, updatedComment);

        if (Comment == null)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
    {
        var Comment = await _commentRepo.DeleteAsync(id);

        if (Comment == null)
            return NotFound();

        return NoContent();
    }
}

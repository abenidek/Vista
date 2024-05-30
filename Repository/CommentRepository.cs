using Microsoft.EntityFrameworkCore;
using Vista.Data.AppDbContext;
using Vista.Data.Models;

namespace Vista;

public class CommentRepository(VistaDbContext _context) : ICommentRepository
{
    private readonly VistaDbContext _context = _context;

    public async Task<Comment> CreateAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return comment;
    }
    public async Task<Comment?> UpdateAsync(Guid id, string updatedComment)
    {
        var Comment = await _context.Comments.FindAsync(id);

        if (Comment is null)
            return null;
        
        Comment.Content = updatedComment;

        await _context.SaveChangesAsync();

        return Comment;
    }
    
    public async Task<Comment?> DeleteAsync(Guid id)
    {
        var Comment = await _context.Comments.FindAsync(id);

        if (Comment is null)
            return null;
        
        _context.Comments.Remove(Comment);
        await _context.SaveChangesAsync();

        return Comment;
    }

}

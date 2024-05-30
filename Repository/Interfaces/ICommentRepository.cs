using Vista.Data.Models;

namespace Vista;

public interface ICommentRepository
{
    Task<Comment> CreateAsync(Comment comment);
    Task<Comment?> UpdateAsync(Guid id, string comment);
    Task<Comment?> DeleteAsync(Guid id);
}

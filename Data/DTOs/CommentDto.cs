using Vista.Data.DTOs;

namespace Vista;

public record CommentDto
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public string PostDate { get; set; } = string.Empty;
    public UserDto? User { get; set; }
}

public record CreateCommentDto
{
    public Guid VideoId { get; set; }
    public required string Content { get; set; }
}

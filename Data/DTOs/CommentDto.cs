﻿namespace Vista;

public record CommentDto
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid VideoId { get; set; }
    public required string UserName { get; set; }
}

public record CreateCommentDto
{
    public Guid VideoId { get; set; }
    public required string Content { get; set; }
}
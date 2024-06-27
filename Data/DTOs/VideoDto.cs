namespace Vista.Data.DTOs;

public record VideoDetailDto
{
    public Guid VideoId { get; set; }
    public string VideoName { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string VideoDescription { get; set; } = string.Empty;
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public int Views { get; set; }
    public string UploadDate { get; set; } = string.Empty;
    public List<CommentDto>? Comments { get; set; }
    public UserDetailDto? User { get; set; }
    public bool IsLiked { get; set; }
    public bool IsDisliked { get; set; }
}

public record VideoSummaryDto
{
    public Guid VideoId { get; set; }
    public string VideoName { get; set; } = string.Empty;
    public string VideoLength { get; set;} = string.Empty;
    public string ThumbnailUrl { get; set;} = string.Empty;
    public int Views { get; set; }
    public string UploadDate { get; set; } = string.Empty;
    public UserDto? User { get; set; }
    public int CategoryId { get; set; }
}

public record MyVideoDto
{
    public Guid VideoId { get; set; }
    public string VideoName { get; set; } = string.Empty;
    public string VideoDescription { get; set;} = string.Empty;
    public string VideoLength { get; set;} = string.Empty;
    public string ThumbnailUrl { get; set;} = string.Empty;
    public int Views { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public string UploadDate { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}

public record CreateVideoDto
{
    public string VideoName { get; set; } = string.Empty;
    public string VideoDescription { get; set; } = string.Empty;
    public required IFormFile VideoFile { get; set; }
    public required IFormFile ThumbnailFile { get; set; }
    public Guid UserId { get; set; }
    public int CategoryId { get; set; }
}

public record UpdateVideoDto
{
    public string VideoName { get; set; } = string.Empty;
    public string VideoDescription { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public required IFormFile ThumbnailFile { get; set; }
}
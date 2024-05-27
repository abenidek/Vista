namespace Vista.Data.DTOs;

public record VideoDetailDto
{
    public Guid VideoId { get; set; }
    public string VideoName { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string VideoDescription { get; set; } = string.Empty;
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public int Views { get; set; }
    public DateTime UploadDate { get; set; }
    public Guid UserId { get; set; }
    public int CategoryId { get; set; }
}

public record VideoSummaryDto
{
    public Guid VideoId { get; set; }
    public string VideoName { get; set; } = string.Empty;
    public string VideoLength { get; set;} = string.Empty;
    public string ThumbnailUrl { get; set;} = string.Empty;
    public int Views { get; set; }
    public DateTime UploadDate { get; set; }
    public Guid UserId { get; set; }
    public int CategoryId { get; set; }
}

public record CreateVideoDto{
    public string VideoName { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string VideoDescription { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set;} = string.Empty;
    public string VideoLength { get; set;} = string.Empty;
    public Guid UserId { get; set; }
    public int CategoryId { get; set; }
}
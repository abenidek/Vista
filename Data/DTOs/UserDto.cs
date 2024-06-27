namespace Vista.Data.DTOs;

public record UserDto
{
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public string ProfilePicUrl { get; set; } = string.Empty;
}

public record UserDetailDto
{
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public string ProfilePicUrl { get; set; } = string.Empty;
    public int FollowersCount { get; set; }
    public bool isUserFollowed { get; set; }
}

public record UserFromGoogleDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public required string UserName { get; set; }
    public string ProfilePicUrl { get; set; } = string.Empty;
}

public record ProfileDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int FollowersCount { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePicUrl { get; set; }
    public List<VideoSummaryDto>? Videos { get; set; }
    public bool IsBeingFollowed { get; set; }
}

public record MyProfileDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
    public int TotalViews { get; set; }
    public int TotalLikes { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePicUrl { get; set; }
    public List<MyVideoDto>? Videos { get; set; }
}

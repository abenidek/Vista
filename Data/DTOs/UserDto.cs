namespace Vista.Data.DTOs;

public record UserDto
{
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public string ProfilePicUrl { get; set; } = string.Empty;

}
public record UserFromGoogleDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public required string UserName { get; set; }
    public required IFormFile ProfilePicFile { get; set; }
}

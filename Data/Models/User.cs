using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vista.Data.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("username")]
    public string UserName { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("dob")]
    public DateOnly DateOfBirth { get; set; }

    public ICollection<Video>? Videos { get; set; }
    public ICollection<LikeAndDislike>? LikesAndDislikes { get; set; }
    public ICollection<WatchedVideo>? WatchedVideos { get; set; }
    public ICollection<Comment>? Comments { get; set; }

}

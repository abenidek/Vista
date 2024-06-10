using System.ComponentModel.DataAnnotations.Schema;

namespace Vista.Data.Models;

[Table("liked_video")]
public class LikedVideo
{
    [Column("user_id")]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    [Column("video_id")]
    [ForeignKey("Video")]
    public Guid VideoId { get; set; }

    public User? User { get; set; }
    public Video? Video { get; set; }
}

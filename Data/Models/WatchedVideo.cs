using System.ComponentModel.DataAnnotations.Schema;

namespace Vista.Data.Models;

[Table("watched_videos")]
public class WatchedVideo
{
    [Column("user_id")]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    [Column("video_id")]
    [ForeignKey("Video")]
    public Guid VideoId { get; set; }

    [Column("viewed_at")]
    public DateTime ViewedAt { get; set; }

    public User? User { get; set; }
    public Video? Video { get; set; }
}

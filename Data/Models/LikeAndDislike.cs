using System.ComponentModel.DataAnnotations.Schema;

namespace Vista.Data.Models;

[Table("likes_and_dislikes")]
public class LikeAndDislike
{
    [Column("user_id")]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    [Column("video_id")]
    [ForeignKey("Video")]
    public Guid VideoId { get; set; }

    [Column("like_or_dislike")]
    public bool LikeOrDislike{ get; set; }

    public User? User { get; set; }
    public Video? Video { get; set; }
}

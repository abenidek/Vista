using System.ComponentModel.DataAnnotations.Schema;
using Vista.Data.Models;

namespace Vista.Data.Models;

[Table("saved_videos")]
public class SavedVideo
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

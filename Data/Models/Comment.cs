using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vista.Data.Models;

[Table("comments")]
public class Comment
{
    [Key]
    [Column("comment_id")]
    public Guid Id { get; set; }

    [Column("content")]
    public required string Content { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("user_id")]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    [Column("video_id")]
    [ForeignKey("Video")]
    public Guid VideoId { get; set; }

    public User? User { get; set; }
    public Video? Video { get; set; }
}

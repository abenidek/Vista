using System.ComponentModel.DataAnnotations.Schema;

namespace Vista.Data.Models;

[Table("followers")]
public class Follower
{
    [Column("user_id")]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    [Column("follower_id")]
    [ForeignKey("User")]
    public Guid FollowerId { get; set; }

    public User? User { get; set; }
    public User? FollowerUser { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace Vista.Data.Models;

[Table("followers")]
public class UserFollower
{
    [Column("followed_user_id")]
    [ForeignKey("FollowedUser")]
    public Guid FollowedUserId { get; set; }

    [Column("follower_user_id")]
    [ForeignKey("FollowerUser")]
    public Guid FollowerUserId { get; set; }

    public User? FollowedUser { get; set; }
    public User? FollowerUser { get; set; }
}
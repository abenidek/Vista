using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vista.Data.Models;

    [Table("videos")]
    public class Video
    {
        [Key]
        [Column("video_id")]
        public Guid VideoId { get; set; }

        [Column("video_name")]
        public string VideoName { get; set;} = string.Empty;

        [Column("video_length")]
        public string VideoLength { get; set;} = string.Empty;

        [Column("video_url")]
        public string VideoUrl { get; set;} = string.Empty;

        [Column("video_thumbnail_url")]
        public string ThumbnailUrl { get; set;} = string.Empty;

        [Column("video_description")]
        public string VideoDescription { get; set; } = string.Empty;

        [Column("likes")]
        public int Likes { get; set; }

        [Column("dislikes")]
        public int Dislikes { get; set; }

        [Column("views")]
        public int Views { get; set; }

        [Column("upload_date")]
        public DateTime UploadDate { get; set; }

        [Column("user_id")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Column("category_id")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public User? User { get; set; }
        public Category? Category{ get; set; }

        public ICollection<LikeAndDislike>? LikesAndDislikes { get; set; }
        public ICollection<WatchedVideo>? WatchedVideos { get; set; }
        public ICollection<Comment>? Comments { get; set; }
     }
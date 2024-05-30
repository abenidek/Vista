using Vista.Data.DTOs;
using Vista.Data.Models;

namespace Vista;

public static class VideoMapper
{
    public static VideoDetailDto ToVideoDetailDto(this Video video)
    {
        return new VideoDetailDto{
            VideoId = video.VideoId,
            VideoName = video.VideoName,
            VideoDescription = video.VideoDescription,
            VideoUrl = video.VideoUrl,
            Views =  video.Views,
            Likes = video.Likes,
            Dislikes = video.Dislikes,
            UploadDate = video.UploadDate,
            Comments = video.Comments!.Select(c => c.ToCommentDto()).ToList(),
            CategoryId = video.CategoryId,
            UserId = video.UserId,
        };
    }

    public static VideoSummaryDto ToVideoSummaryDto(this Video video)
    {
        return new VideoSummaryDto{
            VideoId = video.VideoId,
            VideoName = video.VideoName,
            ThumbnailUrl = video.ThumbnailUrl,
            VideoLength = video.VideoLength,
            Views = video.Views,
            UploadDate = video.UploadDate,
            UserName = video.User!.UserName,
            UserId = video.UserId,
            CategoryId = video.CategoryId,
        };
    }

    public static Video ToVideoModel(this CreateVideoDto createVideoDto)
    {
        return new Video{
            VideoName = createVideoDto.VideoName,
            VideoDescription = createVideoDto.VideoDescription,
            VideoUrl = createVideoDto.VideoUrl,
            ThumbnailUrl = createVideoDto.ThumbnailUrl,
            VideoLength = createVideoDto.VideoLength,
            UserId = createVideoDto.UserId,
            CategoryId = createVideoDto.CategoryId
        };
    }
}

using Vista.Data.DTOs;
using Vista.Data.Models;

namespace Vista.Mappers;

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
            User = video.User!.ToUserDto(),
            CategoryId = video.CategoryId
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
            User = video.User!.ToUserDto(),
            CategoryId = video.CategoryId
        };
    }

    public static Video ToVideoModel(this CreateVideoDto createVideoDto)
    {
        return new Video{
            VideoName = createVideoDto.VideoName,
            VideoDescription = createVideoDto.VideoDescription,
            UserId = createVideoDto.UserId,
            CategoryId = createVideoDto.CategoryId
        };
    }
}

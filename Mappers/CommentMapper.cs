using Vista.Data.Models;

namespace Vista.Mappers;

public static class CommentMapper
{
    public static CommentDto ToCommentDto(this Comment commentModel)
    {
        return new CommentDto{
            Id = commentModel.Id,
            Content = commentModel.Content,
            CreatedAt = commentModel.CreatedAt,
            VideoId = commentModel.VideoId,
            User = commentModel.User!.ToUserDto()
        };
    }

    public static Comment ToCommentModel(this CreateCommentDto commentDto, Guid userId)
    {
        return new Comment
            {
                Content = commentDto.Content,
                VideoId = commentDto.VideoId,
                UserId = userId
            };
    }
}

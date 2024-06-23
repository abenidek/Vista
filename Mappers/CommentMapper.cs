using Vista.Data.Models;

namespace Vista.Mappers;

public static class CommentMapper
{
    public static CommentDto ToCommentDto(this Comment commentModel)
    {
        var dateDifference = DateTime.Now - commentModel.CreatedAt;

        return new CommentDto{
            Id = commentModel.Id,
            Content = commentModel.Content,
            PostDate = dateDifference.FormatDateDifference(),
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

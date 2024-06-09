using Vista.Data.DTOs;
using Vista.Data.Models;

namespace Vista.Mappers;

public static class UserMapper
{
    public static UserDto ToUserDto(this User user){
        return new UserDto
        {
            UserId = user.UserId,
            UserName = user.UserName,
            ProfilePicUrl = user.ProfilePicUrl!
        };
    }

    public static User ToUserModel(this UserFromGoogleDto user)
    {
        return new User
        {
            Name = user.Name,
            Email = user.Email,
            UserName = user.UserName
        };
    }
}

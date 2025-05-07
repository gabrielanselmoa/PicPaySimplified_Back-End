using PicPaySimplified.Application.Dtos.Users;
using PicPaySimplified.Domain.Entities;

namespace PicPaySimplified.Application.Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(User user)
    {
        return new UserResponse()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Cpf = user.Cpf,
            Balance = user.Balance,
            IsActive = true,
            UserType = user.Type
        };
    }

    public static User ToDomain(UserRequest request)
    {
        return new User()
        {
            FullName = request.FullName,
            Email = request.Email,
            Cpf = request.Cpf,
            IsActive = true,
            Password = request.Password,
            Type = request.UserType
        };
    }

    public static UserPreview ToPreview(User user)
    {
        return new UserPreview()
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.FullName,
            Type = user.Type
        };
    }

    public static User UpdateDomain(User user, UserRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.FullName) && user.FullName != request.FullName)
            user.FullName = request.FullName;
        if (!string.IsNullOrWhiteSpace(request.Email) && user.Email != request.Email)
            user.Email = request.Email;
        if (!string.IsNullOrWhiteSpace(request.Cpf) && user.Cpf != request.Cpf)
            user.Cpf = request.Cpf;
        if (!string.IsNullOrWhiteSpace(request.UserType.ToString()) && user.Type != request.UserType)
            user.Type = request.UserType;
        
        return user;
    }
}
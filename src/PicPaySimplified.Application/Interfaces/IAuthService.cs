using PicPaySimplified.Application.Dtos.Auth;
using PicPaySimplified.Domain.Entities;

namespace PicPaySimplified.Application.Interfaces;

public interface IAuthService
{
    Task<ResultModel<bool>> SignUp(SignUpRequest request);
    Task<ResultModel<LoggedResponse>> SignIn(SignInRequest request);
}
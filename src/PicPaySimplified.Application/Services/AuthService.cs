using PicPaySimplified.Application.Dtos.Auth;
using PicPaySimplified.Application.Interfaces;
using PicPaySimplified.Domain.Entities;
using PicPaySimplified.Domain.Interfaces;

namespace PicPaySimplified.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ISecurityService _security;

    public AuthService(ISecurityService securityService, IUserRepository userRepository)
    {
        _security = securityService;
        _userRepository = userRepository;
    }
    
    public async Task<ResultModel<bool>> SignUp(SignUpRequest request)
    {
        var response = new ResultModel<bool>();
        var user = await _userRepository.GetByEmail(request.Email);
        if (user != null)
        {
            response.Data = false;
            response.Message = "User already exists!";
            response.Success = false;
            return response;
        }
        // Converting to Domain + hashingPassword
        var userDomain = new User()
        {
            FullName = request.Name,
            Email = request.Email,
            IsActive = true,
        };
        
        userDomain.Password = _security.HashPassword(userDomain, request.Password);
        await _userRepository.Create(userDomain);
        var token = _security.GenerateJwtToken(userDomain, 1, "Days");
        
        response.Data = true;
        response.Message = "User created successfully!";
        response.Success = true;
        return response;
    }
    
    public async Task<ResultModel<LoggedResponse>> SignIn(SignInRequest request)
    {
        var response = new ResultModel<LoggedResponse>();
        var user = await _userRepository.GetByEmail(request.Email);
        if (user == null)
        {
            response.Data = null;
            response.Message = "Wrong e-mail or password!";
            response.Success = false;
            return response;
        }

        var isPasswordCorrect = _security.VerifyPassword(user, request.Password, user.Password);
        if (!isPasswordCorrect)
        {
            response.Data = null;
            response.Message = "Wrong e-mail or password!";
            response.Success = false;
            return response;
        }

        var token = _security.GenerateJwtToken(user, 1, "Days");
        response.Data = new LoggedResponse{Name = user.FullName!, Email = user.Email, JwToken = token};
        response.Message = "Success";
        response.Success = true;
        return response;
    }
}
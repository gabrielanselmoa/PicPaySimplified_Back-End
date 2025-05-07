using PicPaySimplified.Application.Dtos.Users;
using PicPaySimplified.Application.Interfaces;
using PicPaySimplified.Application.Mappers;
using PicPaySimplified.Domain.Entities;
using PicPaySimplified.Domain.Interfaces;

namespace PicPaySimplified.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResultModel<ICollection<UserResponse?>>> GetAll()
    {
        var response = new ResultModel<ICollection<UserResponse?>>();
        var users = await _userRepository.GetAll();
        if (users.Count == 0)
        {
            response.Message = "No users found";
            response.Success = false;
            return response;
        }
        var dto = users.Select(u => UserMapper.ToResponse(u!)).ToList();
        response.Data = dto!;
        response.Message = "Success";
        response.Success = true;
        
        return response;
    }

    public async Task<ResultModel<UserResponse?>> GetById(string id)
    {
        var response = new ResultModel<UserResponse?>();
        var user = await _userRepository.GetById(id);
        if (user == null)
        {
            response.Message = "User not found";
            response.Success = false;
            return response;
        }
        var dto = UserMapper.ToResponse(user);
        response.Data = dto;
        response.Message = "Success";
        response.Success = true;
        
        return response;
    }

    public async Task<ResultModel<UserResponse?>> GetByEmail(string email)
    {
        var response = new ResultModel<UserResponse?>();
        var user = await _userRepository.GetByEmail(email);
        if (user == null)
        {
            response.Message = "User not found";
            response.Success = false;
            return response;
        }
        var dto = UserMapper.ToResponse(user);
        response.Data = dto;
        response.Message = "Success";
        response.Success = true;
        
        return response;
    }

    public async Task<ResultModel<UserResponse?>> GetByCpf(string cpf)
    {
        var response = new ResultModel<UserResponse?>();
        var user = await _userRepository.GetByCpf(cpf);
        if (user == null)
        {
            response.Message = "User not found";
            response.Success = false;
            return response;
        }
        var dto = UserMapper.ToResponse(user);
        response.Data = dto;
        response.Message = "Success";
        response.Success = true;
        
        return response;
    }

    public async Task<ResultModel<UserResponse?>> Create(UserRequest request)
    {
        var response = new ResultModel<UserResponse?>();
        var user = await _userRepository.GetByEmail(request.Email);
        var userCpf = await _userRepository.GetByCpf(request.Cpf);
        // validation by email and cpf as unique fields!
        if (user != null || userCpf != null)
        {
            response.Message = "User already exists!";
            response.Success = false;
            return response;
        }
        var domain = UserMapper.ToDomain(request);
        await _userRepository.Create(domain);
        var dto = UserMapper.ToResponse(domain);
        
        response.Data = dto;
        response.Message = "Success";
        response.Success = true; 
        return response;
    }

    public async Task<ResultModel<UserResponse?>> Update(string id, UserRequest request)
    {
        var response = new ResultModel<UserResponse?>();
        var user = await _userRepository.GetById(id);
        if (user == null)
        {
            response.Message = "User does not exist!";
            response.Success = false;
            return response;
        }
        var updatedUser = UserMapper.UpdateDomain(user, request);
        await _userRepository.Update(id, updatedUser);
        
        var dto = UserMapper.ToResponse(updatedUser);
        
        response.Data = dto;
        response.Message = "Success";
        response.Success = true; 
        return response;;
    }

    public async Task<ResultModel<bool>> Delete(string id)
    {
        var response = new ResultModel<bool>();
        var user = await _userRepository.GetById(id);
        if (user == null)
        {
            response.Message = "User not found";
            response.Success = false;
            return response;
        }

        await _userRepository.Delete(id);
        response.Message = "Deleted successfully!";
        response.Success = true;
        return response;
    }
}
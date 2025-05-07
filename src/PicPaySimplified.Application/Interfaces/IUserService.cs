using PicPaySimplified.Application.Dtos.Users;
using PicPaySimplified.Domain.Entities;

namespace PicPaySimplified.Application.Interfaces;

public interface IUserService
{
    public Task<ResultModel<ICollection<UserResponse?>>> GetAll();
    public Task<ResultModel<UserResponse?>> GetById(string id);
    public Task<ResultModel<UserResponse?>> GetByEmail(string email);
    public Task<ResultModel<UserResponse?>> GetByCpf(string cpf);
    public Task<ResultModel<UserResponse?>> Create(UserRequest request);
    public Task<ResultModel<UserResponse?>> Update(string id, UserRequest request);
    public Task<ResultModel<bool>> Delete(string id);
}
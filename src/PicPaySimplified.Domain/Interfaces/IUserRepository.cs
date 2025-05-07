using PicPaySimplified.Domain.Entities;

namespace PicPaySimplified.Domain.Interfaces;

public interface IUserRepository
{
    public Task<ICollection<User?>> GetAll();
    public Task<User?> GetById(string id);
    public Task<User?> GetByEmail(string email);
    public Task<User?> GetByCpf(string cpf);
    public Task Create (User user);
    public Task UpdateBalance(string id, decimal balance);
    public Task Update (string id, User user);
    public Task Delete (string id);
}
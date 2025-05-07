using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PicPaySimplified.Domain.Entities;
using PicPaySimplified.Domain.Interfaces;
using PicPaySimplified.Infrastructure.Data;

namespace PicPaySimplified.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;
    
    public UserRepository(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _users = mongoDatabase.GetCollection<User>("users");
    }

    public async Task<ICollection<User?>> GetAll()
    {
       var users = await _users.Find(user => true).ToListAsync();
       if (users.Count == 0) return [];
       return users!;
    }
    
    public async Task<User?> GetById(string id)
    {
        var user  = await _users.Find(user => user.Id.Equals(id)).FirstOrDefaultAsync();
        if  (user == null) return null;
        return user;
    }

    public async Task<User?> GetByEmail(string email)
    {
        var user  = await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        if  (user == null) return null;
        return user;
    }
    
    public async Task<User?> GetByCpf(string cpf)
    {
        var user  = await _users.Find(user => user.Cpf == cpf).FirstOrDefaultAsync();
        if  (user == null) return null;
        return user;
    }

    public async Task Create(User request)
    {
        await _users.InsertOneAsync(request);
    }
    
    public async Task UpdateBalance(string id, decimal balance)
    {
        await _users.UpdateOneAsync(user => user.Id.Equals(id), Builders<User>.Update.Set("Balance", balance));
    }

    public async Task Update(string id, User request)
    {
        // await _users.ReplaceOneAsync(user => user.Id.Equals(id), request);
        await _users.UpdateOneAsync(user => user.Id.Equals(id), Builders<User>.Update
            .Set("full_name", request.FullName)
            .Set("email", request.Email)
            .Set("cpf", request.Cpf)
            .Set("user_type", request.Type));
    }

    public async Task Delete(string id)
    {
        await _users.DeleteOneAsync(user => user.Id.Equals(id));
    }
}
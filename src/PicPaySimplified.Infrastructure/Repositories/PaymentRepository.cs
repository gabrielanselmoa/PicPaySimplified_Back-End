using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PicPaySimplified.Domain.Entities;
using PicPaySimplified.Domain.Interfaces;
using PicPaySimplified.Infrastructure.Data;

namespace PicPaySimplified.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IMongoCollection<Payment> _payments;

    public PaymentRepository(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _payments = mongoDatabase.GetCollection<Payment>("payments");
    }

    public async Task<ICollection<Payment>> GetAll(User user)
    {
        var payments = await _payments.Find(payment => payment.PayerId.Equals(user.Id) || payment.PayeeId.Equals(user.Id))
            .ToListAsync();
        return payments;
    }
    
    public async Task Create(Payment payment)
    {
        await _payments.InsertOneAsync(payment);
    }
}
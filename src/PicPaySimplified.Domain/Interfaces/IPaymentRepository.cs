using PicPaySimplified.Domain.Entities;

namespace PicPaySimplified.Domain.Interfaces;

public interface IPaymentRepository
{
    public Task<ICollection<Payment>> GetAll(User user);
    public Task Create(Payment payment);
}
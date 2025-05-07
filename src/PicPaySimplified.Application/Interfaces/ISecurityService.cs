using PicPaySimplified.Domain.Entities;

namespace PicPaySimplified.Application.Interfaces;

public interface ISecurityService
{
    string GenerateJwtToken(User user, int duration, string timeUnit);
    string HashPassword(User user, string password);
    bool VerifyPassword(User user, string providedPassword, string storedHash);
}
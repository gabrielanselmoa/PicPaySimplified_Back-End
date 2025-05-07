using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PicPaySimplified.Application.Interfaces;
using PicPaySimplified.Domain.Entities;

namespace PicPaySimplified.Infrastructure.Security;

public class SecurityService : ISecurityService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly PasswordHasher<User> _passwordHasher;

    public SecurityService()
    {
        _secretKey = Environment.GetEnvironmentVariable("JWT_SECRET")!;
        _issuer = Environment.GetEnvironmentVariable("ISSUER")!;
        _audience = Environment.GetEnvironmentVariable("AUDIENCE")!;
        _passwordHasher = new PasswordHasher<User>();
    }
    public string GenerateJwtToken(User user, int duration, string timeUnit)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        var unitType = DateTime.UtcNow.AddDays(1);
        if (timeUnit.Equals("min")) unitType = DateTime.UtcNow.AddMinutes(duration);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _issuer,
            Audience = _audience,
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                
            }),
            Expires = unitType,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }
    public bool VerifyPassword(User user, string providedPassword, string storedHash)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, storedHash, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
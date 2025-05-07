using System.ComponentModel.DataAnnotations;

namespace PicPaySimplified.Application.Dtos.Auth;

public record class SignInRequest
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    public string Email { get; init; } = null!;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Password { get; init; } = null!;
}
using System.ComponentModel.DataAnnotations;

namespace PicPaySimplified.Application.Dtos.Auth;

public record class SignUpRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Name { get; init; } = null!;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    public string Email { get; init; } = null!;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
    public string Password { get; init; } = null!;
}
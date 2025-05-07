namespace PicPaySimplified.Application.Dtos.Auth;

public record LoggedResponse
{
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string JwToken { get; init; } = null!;
}

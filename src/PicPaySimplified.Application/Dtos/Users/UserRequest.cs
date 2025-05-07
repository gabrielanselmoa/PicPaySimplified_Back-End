using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PicPaySimplified.Domain.Entities.Enums;

namespace PicPaySimplified.Application.Dtos.Users;

public record UserRequest(
    [Required, StringLength(100, MinimumLength = 10, ErrorMessage = "Name must be between 10 and 100 characters")]
    string FullName, 
    [Required, EmailAddress]
    string Email, 
    [Required]
    string Password,
    [Required, StringLength(14, MinimumLength = 11, ErrorMessage = "CPF or CNPJ must be between 11 and 14 characters")]
    string Cpf, 
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    UserType UserType);
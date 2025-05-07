using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PicPaySimplified.Domain.Entities.Enums;

namespace PicPaySimplified.Application.Dtos.Users;

public record UserUpdate
(
    [StringLength(100, MinimumLength = 10, ErrorMessage = "Name must be between 10 and 100 characters")]
    string? FullName, 
    [EmailAddress]
    string? Email, 
    [StringLength(14, MinimumLength = 11, ErrorMessage = "CPF or CNPJ must be between 11 and 14 characters")]
    string? Cpf, 
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    UserType? UserType
);
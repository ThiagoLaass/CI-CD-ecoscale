using System.Diagnostics.CodeAnalysis;
using EcoScale.src.Models.Abstract;

namespace EcoScale.src.Public.DTOs
{
    public class CreationResponse<T>
    {
        [NotNull]
        public T? Entity { get; set; }
        public string? Token { get; set; }
    }
    
    public class EmailConfirmationRequest
    {
        [NotNull]
        public required string Email { get; set; }

        [NotNull]
        public required string Codigo { get; set; }
    }

    public class LoginResponse
    {
        public string? Token { get; set; }
    }

    public class LoginRequest
    {
        [NotNull]
        public required string Email { get; set; }

        [NotNull]
        public required string Senha { get; set; }
    }
}
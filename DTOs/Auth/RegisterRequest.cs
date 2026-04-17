using ApsMartChat.Models.Enum;

namespace ApsMartChat.DTOs.Auth;

public record RegisterRequest(string Username,
    string Password,
    string DisplayName,
    UserRole Role
    );
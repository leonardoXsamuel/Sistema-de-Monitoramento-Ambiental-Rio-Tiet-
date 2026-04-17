using ApsMartChat.Models.Enum;

namespace ApsMartChat.DTOs.Auth;

public record AuthResponse(string Token, string Username, string DisplayName, UserRole Role);


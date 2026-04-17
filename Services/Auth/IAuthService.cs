using Microsoft.AspNetCore.Identity.Data;

namespace ApsMartChat.Services.Auth;

public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}

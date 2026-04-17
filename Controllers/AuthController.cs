using ApsMartChat.DTOs;
using ApsMartChat.DTOs.Auth;
using ApsMartChat.Services.Auth;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    
    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        var result = await _auth.RegistrarUsuarioAsync(req);
        if (result is null)
            return Conflict(new { message = "Nome de usuário já existe." });

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var result = await _auth.LoginDeUsuarioAsync(req);
        if (result is null)
            return Unauthorized(new { message = "Credenciais inválidas." });

        return Ok(result);
    }
}

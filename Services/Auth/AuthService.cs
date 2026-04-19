
using ApsMartChat.Data;
using ApsMartChat.DTOs.Auth;
using ApsMartChat.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApsMartChat.Services.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<AuthResponse?> RegistrarUsuarioAsync(RegisterRequest req)
    {
        // Verifica se username já existe
        if (await _db.Users.AnyAsync(u => u.Username == req.Username))
            throw new Exception();

        var user = new User
        {
            Username = req.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password), // linha em que a senha é criptografada
            DisplayName = req.DisplayName,
            Role = req.Role
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return new AuthResponse(
            GenerateToken(user),
            user.Username,
            user.DisplayName,
            user.Role
        );
    }

    public async Task<AuthResponse?> LoginDeUsuarioAsync(LoginRequest req)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Username == req.Username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            throw new Exception("Senha errada."); // => criar exception personalizada para esse case

        return new AuthResponse(
            GenerateToken(user),
            user.Username,
            user.DisplayName,
            user.Role
        );
    }

    //  Gera JWT com infos do usuário 
    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("displayName", user.DisplayName)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

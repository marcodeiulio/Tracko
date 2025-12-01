using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using APIs.Data;
using APIs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APIs.Services;

public class AuthService(IConfiguration configuration, ApplicationDbContext context) : IAuthService
{
    public async Task<User?> RegisterAsync(UserDto request)
    {
        if (await context.Users
                .AnyAsync(user => user.Username.ToLower() == request.Username.ToLower()))
            return null;

        var newUser = new User();

        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(newUser, request.Password);

        newUser.Username = request.Username;
        newUser.PasswordHash = hashedPassword;

        context.Users.Add(newUser);
        await context.SaveChangesAsync();

        return newUser;
    }

    public async Task<TokenResponseDto?> LoginAsync(UserDto request)
    {
        if (request.Password == "" || request.Username == "")
            return null;

        var user = await context.Users
            .Include(user => user.Roles)
            .SingleOrDefaultAsync(user => user.Username.ToLower() == request.Username.ToLower());

        if (user is null)
            return null;

        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) ==
            PasswordVerificationResult.Failed)
            return null;

        return await TokenResponseDto(user);
    }

    public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);

        if (user is null)
            return null;

        return await TokenResponseDto(user);
    }

    private async Task<TokenResponseDto> TokenResponseDto(User user)
    {
        return new TokenResponseDto
        {
            AccessToken = CreateJwt(user),
            RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
        };
    }

    private string CreateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username)
        };
        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Auth:Key")!));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            configuration.GetValue<string>("Auth:Issuer"),
            configuration.GetValue<string>("Auth:Audience"),
            expires: DateTime.UtcNow.AddMinutes(5),
            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(3);
        await context.SaveChangesAsync();
        return refreshToken;
    }

    private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await context.Users.FindAsync(userId);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
            return null;

        return user;
    }
}
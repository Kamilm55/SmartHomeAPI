using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Smart_Home_IoT_Device_Management_API.Application.Services;
using Smart_Home_IoT_Device_Management_API.Domain.Entities;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Authentication;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<JwtService> _logger;
    private readonly UserManager<User> _userManager;

    public JwtService(IOptions<JwtSettings> jwtSettings, ILogger<JwtService> logger, UserManager<User> userManager)
    {
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<string> GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var createdAt = DateTime.UtcNow;
        var expiredAt = createdAt.AddMinutes(_jwtSettings.ExpiresInMinutes);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),

            // Custom claims
            new("createdAt", createdAt.ToString("o")),   // ISO 8601 format
            new("expiredAt", expiredAt.ToString("o"))
        };

        // Add Identity role claims
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        //  Add Identity-specific claims (like SecurityStamp)
        var identityClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(identityClaims);

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiredAt,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}
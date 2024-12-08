using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Services.Interface.Utils;
using Domain.Entities.UserAgg;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Utils;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> CreateToken(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles) roleClaims.Add(new Claim(ClaimTypes.Role, role));

        var claims = new List<Claim>
        {
            new("userid", user.Id.ToString()),
            !string.IsNullOrEmpty(user.LastName) ? new Claim(ClaimTypes.GivenName, user.FirstName) : null,
            !string.IsNullOrEmpty(user.FirstName) ? new Claim(ClaimTypes.Surname, user.LastName) : null,
            new("username", user.UserName),
            user.Email != null ? new Claim(ClaimTypes.Email, user.Email) : null
        };

        claims.AddRange(roleClaims);

        var secretKey = _configuration["JwtToken:Secret"];
        var issuer = _configuration["JwtToken:Issuer"];
        var audience = _configuration["JwtToken:Audience"];


        // Use UTF-8 encoding to get bytes from the secret key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(3),
            SigningCredentials = creds,
            Issuer = issuer,
            Audience = audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
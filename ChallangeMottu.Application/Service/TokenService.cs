using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChallangeMottu.Application.Configs;
using ChallangeMottu.Domain;
using Microsoft.IdentityModel.Tokens;

namespace ChallangeMottu.Application.Service;

public class TokenService(JwtSettings jwtSettings) : ITokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
    
    public string GenerateToken(Usuario user)
    {
        var handler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var tokenDescription = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddMinutes(10),
            Subject = GenerateClaims(user)
        };

        var jwt = handler.CreateToken(tokenDescription);
        return handler.WriteToken(jwt);
    }
    
    private static ClaimsIdentity GenerateClaims(Usuario user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Nome)
        };
        
        return new ClaimsIdentity(claims);
    }
}
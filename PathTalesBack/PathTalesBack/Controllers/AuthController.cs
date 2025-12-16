using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PathTalesBack.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] Login user)
    {
        if (user.Email == "admin@admin.ch" && user.Password == "admin")
        {
            var token = GenerateJwtToken(user.Email);

            return Ok(new { token });
        }
        return Unauthorized();
    }

    [HttpPost("check")]
    public IActionResult Check([FromBody] CheckToken token)
    {
        Console.WriteLine(token);
        bool isValid = ValidateJwtToken(token.Token);

        return Ok(isValid);

        /*
        if (isValid)
        {
            return Ok(new { token });
        }
        return Ok(new { token });
        return Unauthorized();
        */
    }

    /// <summary>
    /// Genere le token
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    private string GenerateJwtToken(string email)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myKey"));

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Check si le token est valide
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static bool ValidateJwtToken(string token)
    {
        try
        {
            string secretKey = "myKey";
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            var usernameClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Token validation failed: " + ex.Message);
            return false;
        }
    }
}
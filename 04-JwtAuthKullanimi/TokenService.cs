using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _04_JwtAuthKullanimi
{
    public class TokenService
    {
        public const string SECRETKEY = "6325763C94124E88839FEB30D58DF6CA";
        public const string ISSUER = "api.com";
        public const string AUDIENCE = "api.com";
        public static string GenerateToken(string username)
        {
            byte[] key = Encoding.UTF8.GetBytes(SECRETKEY);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credantials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "99"),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "admin")
                };

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(ISSUER, AUDIENCE, claims, null, DateTime.Now.AddDays(3), credantials);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
    }
}

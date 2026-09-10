using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SCM.ApiAutenticacion.Transversal.Soporte
{
    public static class JwtGenerador
    {
        public static string JwtToken(List<Claim> Claims, string Key, string Issuer, string Audience, int ExpiresSegundo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                    issuer: Issuer,
                    audience: Audience,
                    claims: Claims,
                    expires: DateTime.Now.AddSeconds(ExpiresSegundo),
                    signingCredentials: credentials);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return jwtToken;

        }
    }
}

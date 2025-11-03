using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OefeningIdentity.Models;

namespace OefeningIdentity.Helper
{
    public static class Token
    {
        public static MySettings? MySettings;

        public static JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(MySettings!.Secret!));
            var token = new JwtSecurityToken(
            issuer: MySettings!.ValidIssuer!,
                audience: MySettings!.ValidAudience!,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
            return token;
        }
    }
}

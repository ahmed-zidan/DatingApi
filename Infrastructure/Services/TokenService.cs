using Core;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey key;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            var secretKey = _configuration.GetSection("JwtSetting:JwtKey").Value;
            key = new SymmetricSecurityKey(Encoding.UTF8
                   .GetBytes(secretKey));
        }
        public string GetToken(AppUser user)
        {
            var claims = new Claim[]
           {
                 new Claim(JwtRegisteredClaimNames.NameId,user.Id),
                 new Claim(JwtRegisteredClaimNames.Email,user.Email),
                 new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                 new Claim(JwtRegisteredClaimNames.FamilyName, user.SecondName),
           };
            var signingCredintiels = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature
                );

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(int.Parse(_configuration.GetSection("JwtSetting:ExpiresInDays").Value)),
                SigningCredentials = signingCredintiels,
                Issuer = _configuration.GetSection("JwtSetting:Issuer").Value
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}

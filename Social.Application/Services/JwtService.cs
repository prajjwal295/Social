using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Social.Application.Options;
using Social.Domain.Aggregates.UserProfileAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Social.Application.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        public SecurityToken CreateToken(ClaimsIdentity identity)
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);

            var tokenDescriptor = GetTokenDescriptor(identity, key);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        public string WriteToken(SecurityToken token)
        {
            return tokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity, byte[]? key)
        {
            return new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddHours(2),
                Audience = _jwtSettings.Audiences.FirstOrDefault(),
                Issuer = _jwtSettings.Issuer,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
        }

        public RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(10),
                    Created = DateTime.UtcNow
                };

            }
        }

    }
}

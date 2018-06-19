using FindAndBook.Providers.Contracts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FindAndBook.Providers
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
        private const string issuer = "http://localhost:56260";
        private const string audience = "http://localhost:56260";

        private JwtSecurityTokenHandler tokenHandler;
        private IDateTimeProvider dateTimeProvider;

        public AuthenticationProvider(JwtSecurityTokenHandler tokenHandler, IDateTimeProvider dateTimeProvider)
        {
            this.tokenHandler = tokenHandler;
            this.dateTimeProvider = dateTimeProvider;
        }

        public string GenerateToken(string username)
        {
            DateTime issuedAt = this.dateTimeProvider.GetCurrentTime();
            DateTime expires = this.dateTimeProvider.GetCurrentTime().AddDays(7);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Name, username)
                });

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = this.tokenHandler.CreateJwtSecurityToken(issuer, audience, claimsIdentity,
                issuedAt, expires, signingCredentials: signingCredentials);

            var tokenString = this.tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}

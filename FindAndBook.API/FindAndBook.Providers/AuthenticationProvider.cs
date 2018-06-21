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
        private const string SECURITY_KEY = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
        private const string ISSUER = "http://localhost:56260";
        private const string AUDIENCE = "http://localhost:56260";
        private const string USER_ROLE = "User";
        private const string MANGER_ROLE = "Manager";

        private JwtSecurityTokenHandler tokenHandler;
        private IDateTimeProvider dateTimeProvider;
        private IHttpContextProvider httpContextProvider;

        public AuthenticationProvider(JwtSecurityTokenHandler tokenHandler, IDateTimeProvider dateTimeProvider, IHttpContextProvider httpContextProvider)
        {
            this.tokenHandler = tokenHandler;
            this.dateTimeProvider = dateTimeProvider;
            this.httpContextProvider = httpContextProvider;
        }

        public Guid CurrentUserID
        {
            get
            {
                return Guid.Parse(this.httpContextProvider.CurrentHttpContext.User.Identity.Name);
            }
        }

        public string CurrentUserRole
        {
            get
            {
                return this.httpContextProvider.CurrentHttpContext.User.IsInRole(USER_ROLE) ? USER_ROLE : MANGER_ROLE;
            }
        }

        public string GenerateToken(string userId, string userRole)
        {
            DateTime issuedAt = this.dateTimeProvider.GetCurrentTime();
            DateTime expires = this.dateTimeProvider.GetCurrentTime().AddDays(7);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Name, userId),
                    new Claim(ClaimTypes.Role, userRole)
                });

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(SECURITY_KEY));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = this.tokenHandler.CreateJwtSecurityToken(ISSUER, AUDIENCE, claimsIdentity,
                issuedAt, expires, signingCredentials: signingCredentials);

            var tokenString = this.tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}

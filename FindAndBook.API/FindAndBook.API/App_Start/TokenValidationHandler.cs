using Autofac.Integration.WebApi;
using FindAndBook.Providers.Contracts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FindAndBook.API
{
    public class TokenValidationHandler : ActionFilterAttribute, IAutofacActionFilter
    {
        private const string SECURITY_KEY = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
        private const string VALID_AUDIENCE = "http://localhost:56260";
        private const string VALID_ISSUER = "http://localhost:56260";

        private readonly JwtSecurityTokenHandler handler;
        private readonly IDateTimeProvider dateTimeProvider;

        public TokenValidationHandler(JwtSecurityTokenHandler handler, IDateTimeProvider dateTimeProvider)
        {
            this.handler = handler;
            this.dateTimeProvider = dateTimeProvider;
        }

        public override async Task OnActionExecutingAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            if (ShouldSkipAuthorization(actionContext))
            {
                return;
            }
            else
            {
                var authorizationHeader = actionContext.Request.Headers.Authorization;
                if (authorizationHeader == null)
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                var token = authorizationHeader.Parameter;
                var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(SECURITY_KEY));


                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = VALID_AUDIENCE,
                    ValidIssuer = VALID_ISSUER,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.ValidateLifetime,
                    IssuerSigningKey = securityKey
                };

                SecurityToken securityToken;
                var principal = handler.ValidateToken(token, validationParameters, out securityToken);
                Thread.CurrentPrincipal = principal;
                HttpContext.Current.User = principal;
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        private bool ShouldSkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        private bool ValidateLifetime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            var now = this.dateTimeProvider.GetCurrentTime();

            if (expires != null && now < expires)
            {
                return true;
            }

            return false;
        }
    }
}
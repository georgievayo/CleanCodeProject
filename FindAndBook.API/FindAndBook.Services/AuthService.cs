using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services.Contracts;
using System;
using System.Linq;

namespace FindAndBook.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<Token> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokensFactory factory;
        private readonly IDateTimeProvider dateTimeProvider;

        public AuthService(IRepository<Token> repository, IUnitOfWork unitOfWork, ITokensFactory factory, IDateTimeProvider dateTimeProvider)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.factory = factory;
            this.dateTimeProvider = dateTimeProvider;
        }

        public void DeleteExpiredTokens()
        {
            var expiredTokens = this.GetExpiredTokens();
            foreach (var token in expiredTokens)
            {
                this.repository.Delete(token);
            }

            this.unitOfWork.Commit();
        }

        public void DeleteToken(string tokenValue)
        {
            var token = this.GetTokenByValue(tokenValue);
            if(token != null)
            {
                this.repository.Delete(token);
                this.unitOfWork.Commit();
            }
            else
            {
                throw new ArgumentNullException("Token was not found.");
            }
        }

        public void RenewToken(string tokenValue)
        {
            var token = this.GetTokenByValue(tokenValue);

            if(token != null)
            {
                var currentTime = this.dateTimeProvider.GetCurrentTime();
                token.ExpirationTime = currentTime + new TimeSpan(0, 30, 0);

                this.repository.Update(token);
                this.unitOfWork.Commit();
            }
            else
            {
                throw new ArgumentNullException("Token was not found.");
            }
        }

        public void SaveUserToken(string userId, string tokenValue)
        {
            var currentTime = this.dateTimeProvider.GetCurrentTime();
            var expirationTime = currentTime + new TimeSpan(0, 30, 0);
            var token = this.factory.Create(userId, tokenValue, expirationTime);

            this.repository.Add(token);
            this.unitOfWork.Commit();
        }

        private IQueryable<Token> GetExpiredTokens()
        {
            var currentTime = this.dateTimeProvider.GetCurrentTime();

            return this.repository
                .All
                .Where(token => token.ExpirationTime < currentTime);
        }

        private Token GetTokenByValue(string tokenValue)
        {
            return this.repository
                .All
                .FirstOrDefault(t => t.Value == tokenValue);
        }
    }
}

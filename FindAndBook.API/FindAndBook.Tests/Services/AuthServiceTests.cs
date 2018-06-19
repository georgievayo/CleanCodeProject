using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FindAndBook.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        [Test]
        public void MethodDeleteExpiredTokensShould_CallRepositoryMethodDeleteOnce_WhenThereIsOneExpiredToken()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var expiredToken = new Token()
            {
                UserId = "12345",
                Value = "sdijasdjkads",
                ExpirationTime = new DateTime(2012, 12, 24)
            };
            var tokens = new List<Token>()
            {
                expiredToken,
                new Token()
                {
                    UserId = "12345",
                    Value = "sdijasdjkads",
                    ExpirationTime = DateTime.Now + new TimeSpan(0,20,0)
                }
            };

            repositoryMock.Setup(r => r.All).Returns(() => tokens.AsQueryable());
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime()).Returns(() => DateTime.Now);

            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);
            authService.DeleteExpiredTokens();

            repositoryMock.Verify(r => r.Delete(It.IsAny<Token>()), Times.Once);
        }

        [Test]
        public void MethodDeleteExpiredTokensShould_CallRepositoryMethodDeleteOnCorrectToken()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var expiredToken = new Token()
            {
                UserId = "12345",
                Value = "sdijasdjkads",
                ExpirationTime = new DateTime(2012, 12, 24)
            };
            var tokens = new List<Token>()
            {
                expiredToken,
                new Token()
                {
                    UserId = "12345",
                    Value = "sdijasdjkads",
                    ExpirationTime = DateTime.Now + new TimeSpan(0,20,0)
                }
            };

            repositoryMock.Setup(r => r.All).Returns(() => tokens.AsQueryable());
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime()).Returns(() => DateTime.Now);

            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);
            authService.DeleteExpiredTokens();

            repositoryMock.Verify(r => r.Delete(expiredToken), Times.Once);
        }

        [Test]
        public void MethodDeleteExpiredTokensShould_CallUnitOfWorkMethodCommit()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var expiredToken = new Token()
            {
                UserId = "12345",
                Value = "sdijasdjkads",
                ExpirationTime = new DateTime(2012, 12, 24)
            };
            var tokens = new List<Token>()
            {
                expiredToken,
                new Token()
                {
                    UserId = "12345",
                    Value = "sdijasdjkads",
                    ExpirationTime = DateTime.Now + new TimeSpan(0,20,0)
                }
            };

            repositoryMock.Setup(r => r.All).Returns(() => tokens.AsQueryable());
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime()).Returns(() => DateTime.Now);

            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);
            authService.DeleteExpiredTokens();

            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Test]
        public void MethodDeleteExpiredTokensShould_NotCallRepositoryMethodDelete_WhenThereIsNoExpiredTokens()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var tokens = new List<Token>()
            {
                new Token()
                {
                    UserId = "1234543243324",
                    Value = "432442342",
                    ExpirationTime = DateTime.Now + new TimeSpan(0,20,0)
                },
                new Token()
                {
                    UserId = "12345",
                    Value = "sdijasdjkads",
                    ExpirationTime = DateTime.Now + new TimeSpan(0,20,0)
                }
            };

            repositoryMock.Setup(r => r.All).Returns(() => tokens.AsQueryable());
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime()).Returns(() => DateTime.Now);

            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);
            authService.DeleteExpiredTokens();

            repositoryMock.Verify(r => r.Delete(It.IsAny<Token>()), Times.Never);
        }

        [Test]
        public void MethodDeleteTokenShould_DeleteCorrectToken()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var tokenValueToDelete = "token";
            var tokenToDelete = new Token()
            {
                UserId = "12345",
                Value = tokenValueToDelete,
                ExpirationTime = DateTime.Now + new TimeSpan(0, 20, 0)
            };

            var tokens = new List<Token>()
            {
                new Token()
                {
                    UserId = "1234543243324",
                    Value = "432442342",
                    ExpirationTime = DateTime.Now + new TimeSpan(0,20,0)
                },
                tokenToDelete
            };

            repositoryMock.Setup(r => r.All).Returns(() => tokens.AsQueryable());
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime()).Returns(() => DateTime.Now);

            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);
            authService.DeleteToken(tokenValueToDelete);

            repositoryMock.Verify(r => r.Delete(tokenToDelete), Times.Once);
        }

        [Test]
        public void MethodDeleteTokenShould_CallUnitOfWorkMethodCommit_WhenTokenWasFound()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var tokenValueToDelete = "token";
            var tokenToDelete = new Token()
            {
                UserId = "12345",
                Value = tokenValueToDelete,
                ExpirationTime = DateTime.Now + new TimeSpan(0, 20, 0)
            };

            var tokens = new List<Token>()
            {
                new Token()
                {
                    UserId = "1234543243324",
                    Value = "432442342",
                    ExpirationTime = DateTime.Now + new TimeSpan(0,20,0)
                },
                tokenToDelete
            };

            repositoryMock.Setup(r => r.All).Returns(() => tokens.AsQueryable());
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime()).Returns(() => DateTime.Now);

            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);
            authService.DeleteToken(tokenValueToDelete);

            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Test]
        public void MethodDeleteTokenShould_ThrowArgumentNullException_WhenTokenWasNotFound()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var tokenValueToDelete = "token1";

            var tokens = new List<Token>()
            {
                new Token()
                {
                    UserId = "1234543243324",
                    Value = "432442342",
                    ExpirationTime = DateTime.Now + new TimeSpan(0,20,0)
                },
                new Token()
                {
                    UserId = "12345",
                    Value = "token",
                    ExpirationTime = DateTime.Now + new TimeSpan(0, 20, 0)
                }
            };

            repositoryMock.Setup(r => r.All).Returns(() => tokens.AsQueryable());
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime()).Returns(() => DateTime.Now);

            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);


            Assert.Throws<ArgumentNullException>(() => authService.DeleteToken(tokenValueToDelete));
        }

        [Test]
        public void MethodRenewTokenShould_CallRepositoryMethodUpdate_WhenTokenWasFound()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var tokenValueToUpdate = "token";
            var tokenToUpdate = new Token()
            {
                UserId = "12345",
                Value = tokenValueToUpdate,
                ExpirationTime = DateTime.Now + new TimeSpan(0, 20, 0)
            };

            var tokens = new List<Token>()
            {
                new Token()
                {
                    UserId = "1234543243324",
                    Value = "432442342",
                    ExpirationTime = DateTime.Now + new TimeSpan(0,20,0)
                },
                tokenToUpdate
            };

            repositoryMock.Setup(r => r.All).Returns(() => tokens.AsQueryable());
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime()).Returns(() => DateTime.Now);

            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);
            authService.RenewToken(tokenValueToUpdate);

            repositoryMock.Verify(r => r.Update(tokenToUpdate), Times.Once);
        }

        [Test]
        public void MethodRenewTokenShould_CallUnitOfWorkMethodCommit_WhenTokenWasFound()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var tokenValueToUpdate = "token";
            var tokenToUpdate = new Token()
            {
                UserId = "12345",
                Value = tokenValueToUpdate,
                ExpirationTime = DateTime.Now + new TimeSpan(0, 20, 0)
            };

            var tokens = new List<Token>()
            {
                new Token()
                {
                    UserId = "1234543243324",
                    Value = "432442342",
                    ExpirationTime = DateTime.Now + new TimeSpan(0,20,0)
                },
                tokenToUpdate
            };

            repositoryMock.Setup(r => r.All).Returns(() => tokens.AsQueryable());
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime()).Returns(() => DateTime.Now);

            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);
            authService.RenewToken(tokenValueToUpdate);

            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Test]
        public void MethodRenewTokenShould_ThrowArgumentNullException_WhenTokenWasNotFound()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var tokenValueToUpdate = "token1";

            var tokens = new List<Token>()
            {
                new Token()
                {
                    UserId = "1234543243324",
                    Value = "432442342",
                    ExpirationTime = DateTime.Now + new TimeSpan(0,20,0)
                },
                new Token()
                {
                    UserId = "12345",
                    Value = "token",
                    ExpirationTime = DateTime.Now + new TimeSpan(0, 20, 0)
                }
            };

            repositoryMock.Setup(r => r.All).Returns(() => tokens.AsQueryable());
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime()).Returns(() => DateTime.Now);

            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);

            Assert.Throws<ArgumentNullException>(() => authService.RenewToken(tokenValueToUpdate));
        }

        [Test]
        public void MethodSaveUserTokenShould_CallDateTimeProviderMethodGetCurrentDateTime()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime()).Returns(() => DateTime.Now);

            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);
            authService.SaveUserToken("123456", "token");

            dateTimeProviderMock.Verify(dt => dt.GetCurrentTime(), Times.Once);
        }

        [Test]
        public void MethodSaveUserTokenShould_CallFactoryMethodCreate()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var currentDateTime = DateTime.Now;
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime()).Returns(() => currentDateTime);

            var userId = "123456";
            var tokenValue = "token";
            var expirationTime = currentDateTime + new TimeSpan(0, 30, 0);
            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);
            authService.SaveUserToken(userId, tokenValue);

            factoryMock.Verify(f => f.Create(userId, tokenValue, expirationTime), Times.Once);
        }

        [Test]
        public void MethodSaveUserTokenShould_CallRepositoryMethodAdd()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var currentDateTime = DateTime.Now;
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime())
                .Returns(() => currentDateTime);

            var newToken = new Token()
            {
                UserId = "123456",
                Value = "token",
                ExpirationTime = currentDateTime + new TimeSpan(0, 30, 0)
            };

            factoryMock.Setup(f => f.Create(newToken.UserId, newToken.Value, newToken.ExpirationTime))
                .Returns(() => newToken);


            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);
            authService.SaveUserToken(newToken.UserId, newToken.Value);

            repositoryMock.Verify(r => r.Add(newToken), Times.Once);
        }

        [Test]
        public void MethodSaveUserTokenShould_CallUnitOfWorkMethodCommit()
        {
            var repositoryMock = new Mock<IRepository<Token>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITokensFactory>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var currentDateTime = DateTime.Now;
            dateTimeProviderMock.Setup(dt => dt.GetCurrentTime())
                .Returns(() => currentDateTime);

            var newToken = new Token()
            {
                UserId = "123456",
                Value = "token",
                ExpirationTime = currentDateTime + new TimeSpan(0, 30, 0)
            };

            factoryMock.Setup(f => f.Create(newToken.UserId, newToken.Value, newToken.ExpirationTime))
                .Returns(() => newToken);


            var authService = new AuthService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object, dateTimeProviderMock.Object);
            authService.SaveUserToken(newToken.UserId, newToken.Value);

            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}

using FindAndBook.API.Controllers;
using FindAndBook.API.Mapper;
using FindAndBook.API.Models;
using FindAndBook.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services.Contracts;
using Moq;
using NUnit.Framework;
using System;
using System.Web.Http.Results;

namespace FindAndBook.Tests.API
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUsersService> usersServiceMock;
        private Mock<IAuthenticationProvider> authProviderMock;
        private Mock<IModelsMapper> mapperMock;
        private UsersController controller;
        private User user;
        private string token = "token";
        private Guid currentUserId = Guid.NewGuid();

        [Test]
        public void ActionRegisterShould_ReturnInvalidModelStateResult_WhenModelIsNull()
        {
            var result = controller.Register(null);
            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void ActionRegisterShould_ReturnInvalidModelStateResult_WhenModelIsNotValid()
        {
            var errorMessage = "Username should has at least 6 characters.";

            controller.ModelState.AddModelError("Username", errorMessage);
            var model = new UserModel()
            {
                Username = "test",
                Password = "test",
                Email = "test@email.com",
                FirstName = "Test",
                LastName = "Test",
                PhoneNumber = "085565226114"
            };

            var result = controller.Register(model);

            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void ActionRegisterShould_CallServiceMethodGetByUsername_WhenModelStateIsValid()
        {
            usersServiceMock.Setup(s => s.GetByUsername(It.IsAny<string>()))
                .Returns(() => null);

            var model = new UserModel()
            {
                Username = "test",
                Password = "test",
                Email = "test@email.com",
                FirstName = "Test",
                LastName = "Test",
                PhoneNumber = "085565226114"
            };

            var result = controller.Register(model);

            usersServiceMock.Verify(s => s.GetByUsername(model.Username), Times.Once);
        }

        [Test]
        public void ActionRegisterShould_ReturnConflict_WhenThereIsAlreadyUserWithThatUsername()
        {
            usersServiceMock.Setup(s => s.GetByUsername(It.IsAny<string>()))
                .Returns(() => user);

            var model = new UserModel()
            {
                Username = "test",
                Password = "test",
                Email = "test@email.com",
                FirstName = "Test",
                LastName = "Test",
                PhoneNumber = "085565226114"
            };

            var result = controller.Register(model);

            Assert.IsInstanceOf<NegotiatedContentResult<string>>(result);
        }

        [Test]
        public void ActionRegisterShould_CallServiceMethodCreate_WhenThereIsNoUserWithSameUsername()
        {
            usersServiceMock.Setup(s => s.GetByUsername(It.IsAny<string>()))
                .Returns(() => null);

            var model = new UserModel()
            {
                Username = "test",
                Password = "test",
                Email = "test@email.com",
                FirstName = "Test",
                LastName = "Test",
                PhoneNumber = "085565226114",
                Role = "User"
            };

            var result = controller.Register(model);

            usersServiceMock.Verify(s => s.Create(model.Username, model.Password, model.Email, model.FirstName, model.LastName, model.PhoneNumber, model.Role), Times.Once);
        }

        [Test]
        public void ActionRegisterShould_ReturnOk_WhenUserWasCreated()
        {
            usersServiceMock.Setup(s => s.GetByUsername(It.IsAny<string>()))
                .Returns(() => null);

            var model = new UserModel()
            {
                Username = "test",
                Password = "test",
                Email = "test@email.com",
                FirstName = "Test",
                LastName = "Test",
                PhoneNumber = "085565226114",
                Role = "User"
            };

            var result = controller.Register(model);

            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<UserProfileModel>), result);
        }

        [Test]
        public void ActionRegisterShould_CallMapper_WhenUserWasCreated()
        {
            usersServiceMock.Setup(s => s.GetByUsername(It.IsAny<string>()))
                .Returns(() => null);

            var model = new UserModel()
            {
                Username = "test",
                Password = "test",
                Email = "test@email.com",
                FirstName = "Test",
                LastName = "Test",
                PhoneNumber = "085565226114",
                Role = "User"
            };

            var result = controller.Register(model);

            mapperMock.Verify(m => m.MapUser(user));
        }

        [Test]
        public void ActionLoginShould_ReturnInvalidModelStateResult_WhenModelIsNull()
        {
            var result = controller.Login(null);
            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void ActionLoginShould_ReturnInvalidModelStateResult_WhenModelIsNotValid()
        {
            var errorMessage = "Username should has at least 6 characters.";

            controller.ModelState.AddModelError("Username", errorMessage);
            var model = new LoginModel()
            {
                Username = "test",
                Password = "test"
            };

            var result = controller.Login(model);

            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void ActionLoginShould_CallServiceMethodGetByUsernameAndPassword_WhenModelIsValid()
        {
            var model = new LoginModel()
            {
                Username = "test",
                Password = "test"
            };

            var result = controller.Login(model);

            usersServiceMock.Verify(s => s.GetByUsernameAndPassword(model.Username, model.Password), Times.Once);
        }

        [Test]
        public void ActionLoginShould_ReturnNotFound_WhenUserWasNotFound()
        {
            var model = new LoginModel()
            {
                Username = "test",
                Password = "test"
            };

            usersServiceMock.Setup(s => s.GetByUsernameAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => null);

            var result = controller.Login(model);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void ActionLoginShould_CallAuthProviderMethodGenerateToken_WhenUserWasFound()
        {
            var model = new LoginModel()
            {
                Username = "test",
                Password = "test"
            };

            var result = controller.Login(model);

            authProviderMock.Verify(ap => ap.GenerateToken(It.IsAny<string>(), user.Role.ToString()), Times.Once);
        }

        [Test]
        public void ActionLoginShould_ReturnGeneratedToken_WhenUserWasLoggedIn()
        {
            var model = new LoginModel()
            {
                Username = "test",
                Password = "test"
            };

            var result = controller.Login(model);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<string>), result);
        }

        [Test]
        public void ActionGetProfileShould_ReturnBadRequest_WhenPassedUserIdIsNull()
        {
            var result = controller.GetProfile(null);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void ActionGetProfileShould_GetCurrentUserId_WhenPassedUserIdIsCorrect()
        {
            var userId = Guid.NewGuid();

            var result = controller.GetProfile(userId);

            authProviderMock.Verify(ap => ap.CurrentUserID, Times.Once);
        }

        [Test]
        public void GetProfileShould_GetCurrentUserRole_WhenCurrentUserWantsToSeeHisProfile()
        {
            var result = controller.GetProfile(currentUserId);

            authProviderMock.Verify(ap => ap.CurrentUserRole, Times.Once);
        }

        [Test]
        public void GetProfileShould_ReturnForbidden_WhenCurrentUserAndFoundUserAreNotSame()
        {
            var userId = Guid.NewGuid();
            var result = controller.GetProfile(userId);

            Assert.IsInstanceOf<NegotiatedContentResult<string>>(result);
        }

        [Test]
        public void GetProfileShould_CallServiceMethodGetManager_WhenCurrentUserIsManager()
        {
            authProviderMock.Setup(ap => ap.CurrentUserRole)
                .Returns(() => "Manager");

            var result = controller.GetProfile(currentUserId);

            usersServiceMock.Verify(s => s.GetManager(currentUserId), Times.Once);
        }

        [Test]
        public void GetProfileShould_ReturnNotFound_WhenManagerWasNotFound()
        {
            authProviderMock.Setup(ap => ap.CurrentUserRole)
                .Returns(() => "Manager");
            usersServiceMock.Setup(s => s.GetManager(currentUserId))
                .Returns(() => null);

            var result = controller.GetProfile(currentUserId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GetProfileShould_CallMapper_WhenManagerWasFound()
        {
            authProviderMock.Setup(ap => ap.CurrentUserRole)
                .Returns(() => "Manager");
            var manager = new Manager();
            usersServiceMock.Setup(s => s.GetManager(currentUserId))
                .Returns(() => manager);

            var result = controller.GetProfile(currentUserId);

            mapperMock.Verify(m => m.MapManager(manager), Times.Once);
        }

        [Test]
        public void GetProfileShould_ReturnOkWirhManagerProfile_WhenManagerWasFound()
        {
            authProviderMock.Setup(ap => ap.CurrentUserRole)
                .Returns(() => "Manager");
            var manager = new Manager();
            usersServiceMock.Setup(s => s.GetManager(currentUserId))
                .Returns(() => manager);

            var result = controller.GetProfile(currentUserId);

            Assert.IsInstanceOf<OkNegotiatedContentResult<ManagerProfileModel>>(result);
        }

        [Test]
        public void GetProfileShould_CallServiceMethodGetUser_WhenCurrentUserHasUserRole()
        {
            authProviderMock.Setup(ap => ap.CurrentUserRole)
                .Returns(() => "User");

            var result = controller.GetProfile(currentUserId);

            usersServiceMock.Verify(s => s.GetUser(currentUserId), Times.Once);
        }

        [Test]
        public void GetProfileShould_ReturnNotFound_WhenUserWasNotFound()
        {
            authProviderMock.Setup(ap => ap.CurrentUserRole)
                .Returns(() => "User");
            usersServiceMock.Setup(s => s.GetUser(currentUserId))
                .Returns(() => null);

            var result = controller.GetProfile(currentUserId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GetProfileShould_CallMapper_WhenUserWasFound()
        {
            authProviderMock.Setup(ap => ap.CurrentUserRole)
                .Returns(() => "User");
            usersServiceMock.Setup(s => s.GetUser(currentUserId))
                .Returns(() => user);

            var result = controller.GetProfile(currentUserId);

            mapperMock.Verify(m => m.MapUser(user), Times.Once);
        }

        [Test]
        public void GetProfileShould_ReturnOkWithUserProfile_WhenUserWasFound()
        {
            authProviderMock.Setup(ap => ap.CurrentUserRole)
                .Returns(() => "User");
            usersServiceMock.Setup(s => s.GetUser(currentUserId))
                .Returns(() => user);

            var result = controller.GetProfile(currentUserId);

            Assert.IsInstanceOf<OkNegotiatedContentResult<UserProfileModel>>(result);
        }

        [Test]
        public void ActionDeleteUserShould_ReturnBadRequest_WhenUserIdIsNull()
        {
            var result = controller.DeleteUser(null);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void ActionDeleteUserShould_GetCurrentUserId_WhenUserIdIsValid()
        {
            var userId = user.Id;
            var result = controller.DeleteUser(userId);

            authProviderMock.Verify(ap => ap.CurrentUserID, Times.Once);
        }

        [Test]
        public void ActionDeleteUserShould_ReturnForbidden_WhenCurrentUserAndUserToDeleteAreNotSame()
        {
            var userId = user.Id;

            var result = controller.DeleteUser(userId);

            Assert.IsInstanceOf<NegotiatedContentResult<string>>(result);
        }

        [Test]
        public void ActionDeleteUserShould_CallServiceMethodDetele_WhenCurrentUserWantsToDeleteHisProfile()
        {
            var userId = currentUserId;

            var result = controller.DeleteUser(userId);

            usersServiceMock.Verify(s => s.Delete(userId), Times.Once);
        }

        [Test]
        public void ActionDeleteUserShould_ReturnNotFound_WhenUserToDeleteWasNotFound()
        {
            var userId = currentUserId;
            usersServiceMock.Setup(s => s.GetById(userId))
                .Returns(() => null);

            var result = controller.DeleteUser(userId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        public void ActionDeleteUserShould_ReturnOk_WhenCurrentUserWantsToDeleteHisProfile()
        {
            var userId = currentUserId;

            var result = controller.DeleteUser(userId);

            Assert.IsInstanceOf<OkResult>(result);
        }

        [SetUp]
        public void SetUp()
        {
            usersServiceMock = new Mock<IUsersService>();
            authProviderMock = new Mock<IAuthenticationProvider>();
            mapperMock = new Mock<IModelsMapper>();

            user = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "test",
                Password = "test",
                Email = "test@email.com",
                FirstName = "Test",
                LastName = "Test",
                PhoneNumber = "085565226114"
            };
            usersServiceMock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => user);
            usersServiceMock.Setup(s => s.GetByUsernameAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => user);
            usersServiceMock.Setup(s => s.GetByUsername(It.IsAny<string>()))
                .Returns(() => user);
            authProviderMock.Setup(ap => ap.GenerateToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => token);
            authProviderMock.Setup(ap => ap.CurrentUserID)
                .Returns(() => currentUserId);

            controller = new UsersController(usersServiceMock.Object, authProviderMock.Object, mapperMock.Object);
        }
    }
}

using FindAndBook.API.Controllers;
using FindAndBook.API.Mapper;
using FindAndBook.API.Models;
using FindAndBook.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services.Contracts;
using Moq;
using NUnit.Framework;
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

            Assert.IsInstanceOf<ConflictResult>(result);
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

        [SetUp]
        public void SetUp()
        {
            usersServiceMock = new Mock<IUsersService>();
            authProviderMock = new Mock<IAuthenticationProvider>();
            mapperMock = new Mock<IModelsMapper>();

            user = new User()
            {
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
            authProviderMock.Setup(ap => ap.GenerateToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => token);

            controller = new UsersController(usersServiceMock.Object, authProviderMock.Object, mapperMock.Object);
        }
    }
}

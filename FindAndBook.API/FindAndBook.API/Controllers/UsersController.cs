using FindAndBook.API.Mapper;
using FindAndBook.API.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services.Contracts;
using System;
using System.Web.Http;

namespace FindAndBook.API.Controllers
{
    public class UsersController : ApiController
    {
        private const string MANAGER_ROLE = "Manager";
        private readonly IUsersService usersService;
        private readonly IAuthenticationProvider authProvider;
        private readonly IModelsMapper mapper;

        public UsersController(IUsersService usersService, IAuthenticationProvider authProvider,
            IModelsMapper mapper)
        {
            this.usersService = usersService;
            this.authProvider = authProvider;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("api/users")]
        [AllowAnonymous]
        public IHttpActionResult Register(UserModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = this.usersService.GetByUsername(model.Username);
            if (existingUser != null)
            {
                return Conflict();
            }

            var createdUser = this.usersService.Create(model.Username, model.Password, model.Email,
            model.FirstName, model.LastName, model.PhoneNumber, model.Role);

            var response = this.mapper.MapUser(createdUser);

            return Ok(response);
        }

        [HttpPost]
        [Route("api/login")]
        [AllowAnonymous]
        public IHttpActionResult Login(LoginModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = this.usersService.GetByUsernameAndPassword(model.Username, model.Password);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var userId = user.Id.ToString();
                var userRole = user.Role.ToString();

                var token = this.authProvider.GenerateToken(userId, userRole);

                return Ok(token);
            }
        }

        [HttpGet]
        [Route("api/users/{id}")]
        public IHttpActionResult GetProfile([FromUri]Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var currentUserId = this.authProvider.CurrentUserID;

            if (id == currentUserId)
            {
                var currentUserRole = this.authProvider.CurrentUserRole;
                if (currentUserRole == MANAGER_ROLE)
                {
                    var foundManager = this.usersService.GetManager((Guid)id);

                    if (foundManager == null)
                    {
                        return NotFound();
                    }

                    var response = this.mapper.MapManager(foundManager);

                    return Ok(response);
                }
                else
                {
                    var foundUser = this.usersService.GetUser((Guid)id);
                    if (foundUser == null)
                    {
                        return NotFound();
                    }

                    var response = this.mapper.MapUser(foundUser);

                    return Ok(response);
                }
            }
            else
            {
                return Content(System.Net.HttpStatusCode.Forbidden, "You can see only your profile.");
            }
        }

        [HttpDelete]
        [Route("api/users/{id}")]
        public IHttpActionResult DeleteUser([FromUri]Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var currentUserId = this.authProvider.CurrentUserID;

            if (id == currentUserId)
            {
                var isDeleted = this.usersService.Delete((Guid)id);
                if (isDeleted)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return Content(System.Net.HttpStatusCode.Forbidden, "You can delete only your profile.");
            }
        }
    }
}
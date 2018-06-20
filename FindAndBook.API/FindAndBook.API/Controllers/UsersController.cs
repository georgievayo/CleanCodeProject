using FindAndBook.API.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services.Contracts;
using System;
using System.Web;
using System.Web.Http;

namespace FindAndBook.API.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUsersService usersService;
        private readonly IAuthenticationProvider authProvider;

        public UsersController(IUsersService usersService, IAuthenticationProvider authProvider)
        {
            this.usersService = usersService;
            this.authProvider = authProvider;
        }

        [HttpPost]
        [Route("api/users")]
        [AllowAnonymous]
        public IHttpActionResult Register(UserModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = this.usersService.Create(model.Username, model.Password, model.Email,
            model.FirstName, model.LastName, model.PhoneNumber);

            return Ok("Successful registration.");
        }

        [HttpPost]
        [Route("api/login")]
        [AllowAnonymous]
        public IHttpActionResult Login(LoginModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = this.usersService.GetByUsernameAndPassword(model.Username, model.Password);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var userId = user.Id.ToString();
                var token = this.authProvider.GenerateToken(userId);
                var response = new { token = token };
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("api/users/{username}")]
        public IHttpActionResult GetProfile([FromUri]string username)
        {
            var currentUserId = this.authProvider.CurrentUserID;

            if (username == null || String.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            var user = this.usersService.GetByUsername(username);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                if (user.Id.ToString() != currentUserId)
                {
                    return Content(System.Net.HttpStatusCode.Forbidden, "You cannot see this page.");
                }

                var response = new
                {
                    Username = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                };

                return Ok(response);
            }
        }

        [HttpDelete]
        [Route("api/users/{userId}")]
        public IHttpActionResult DeleteUser([FromUri]string userId)
        {
            var currentUserId = this.authProvider.CurrentUserID;

            if (userId == null || String.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var isDeleted = this.usersService.Delete(Guid.Parse(userId));
            if (isDeleted)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
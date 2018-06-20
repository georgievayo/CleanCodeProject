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

            try
            {
                var user = this.usersService.Create(model.Username, model.Password, model.Email,
                model.FirstName, model.LastName, model.PhoneNumber);

                return Ok("Successful registration.");
            }
            catch (Exception ex)
             {
                return BadRequest("Server error.");
            }
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
                var token = this.authProvider.GenerateToken(user.UserName);
                var response = new { token = token };
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("api/users/{username}")]
        public IHttpActionResult GetProfile([FromUri]string username)
        {
            if (username == null || String.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            var user = this.usersService.GetByUsername(username);
            if(user == null)
            {
                return NotFound();
            }
            else
            {
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
    }
}
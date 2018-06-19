using FindAndBook.API.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services.Contracts;
using System;
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
        public IHttpActionResult Login(LoginModel model)
        {
            if(model == null || !ModelState.IsValid)
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

                return Ok(token);
            }
        }
    }
}
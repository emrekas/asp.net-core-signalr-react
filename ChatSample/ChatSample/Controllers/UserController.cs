using ChatSample.IServices;
using ChatSample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatSample.Controllers {
    [Authorize]
    [ApiController]
    [Route ("[controller]")]
    public class UserController : ControllerBase {
        private readonly IUserService _userService;
        public UserController (IUserService userService) => _userService = userService;

        [AllowAnonymous]
        [HttpPost ("authenticate")]
        public IActionResult Authenticate ([FromBody] AuthenticateModel model) {
            var user = _userService.Authenticate (model.Username, model.Password);

            if (user == null)
                return BadRequest (new { message = "Username or password is incorrect" });
                
            return Ok (user);
        }
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            return Ok(new []{"Yunus","Emre","KAS"});
        }
    }
}
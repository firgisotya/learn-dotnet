using learn.Models.Auth;
using learn.Models.Users;
using learn.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace learn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthRequest model)
        {
            var user = await _authService.Authenticate(model);
            if (user == null)
                return BadRequest(new { message = "Email or password is incorrect" });
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateRequest model)
        {
            var user = await _authService.Register(model);
            if (user == null)
                return BadRequest(new { message = "Email is already taken" });
            return Ok(user);
        }
    }
}

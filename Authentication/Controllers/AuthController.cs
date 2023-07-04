using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly JWTTokenService _jwtTokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(JWTTokenService jwtTokenService, ILogger<AuthController> logger)
        {
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel user)
        {
            _logger.LogInformation("Login method called for username : {login}",user.UserName);
            try
            {
                var loginResult = await _jwtTokenService.GenerateAuthToken(user);

                return loginResult is null ? Unauthorized() : Ok(loginResult);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Login method failed with exception : {ex}", ex.InnerException);
                return Unauthorized();
            }
        }
    }
}

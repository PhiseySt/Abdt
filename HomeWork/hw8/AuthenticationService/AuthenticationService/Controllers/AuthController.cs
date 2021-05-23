using AuthenticationBase.Models;
using AuthenticationService.Entities.Models;
using AuthenticationService.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            return await _authService.SignUp(signUpModel);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            return await _authService.Login(loginModel);
        }
    }
}

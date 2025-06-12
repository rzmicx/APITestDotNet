using Azure.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using APITestDotNet.Data.Models;
using APITestDotNet.Services;
using APITestDotNet.Interfaces;

namespace APITestDotNet.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromForm] LoginModel login)
        {
            var result = _authService.Login(login.username, login.password);
            if (result == "Login berhasil.") {
                var token = _authService.GenerateJwtToken(login.username);
                return Ok(new {token });
            }
            return BadRequest(new { message = result });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisUser model)
        {
            try
            {
              
                var result = await _authService.Register(model.Username, model.Passcode);
                if (result == "ok")
                    return Ok(new { message = "simpan data success" });
                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(nameof(Register), ex);
                return BadRequest(new { message = "mohon hubungi tim support" });
            }
        }
    }
}

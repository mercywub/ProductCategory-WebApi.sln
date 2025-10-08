using Microsoft.AspNetCore.Mvc;
using ProductCategory_WebApi.Application.Dtos;
using ProductCategory_WebApi.Application.Services;

namespace ProductCategory_WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            try
            {
                var user = await _service.RegisterAsync(dto);
                if (user == null) return BadRequest("Email already exists.");
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var user = await _service.LoginAsync(dto);
            if (user == null) return Unauthorized("Invalid credentials.");
            return Ok(user);
        }

        [HttpPost("request-reset-password")]
        public async Task<IActionResult> RequestResetPassword( [FromQuery] string email)
        {
            try
            {
                await _service.GenerateOtpAsync(  email);
                return Ok("OTP sent to email (valid for 30 seconds).");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordOtpDto dto)
        {
            try
            {
                var success = await _service.ResetPasswordAsync(dto.Email, dto.OtpCode, dto.NewPassword);
                if (!success) return BadRequest("Invalid OTP or expired.");
                return Ok("Password reset successful.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

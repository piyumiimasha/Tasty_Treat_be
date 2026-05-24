using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IPasswordResetService _passwordResetService;
        private readonly IUserRepository _userRepository;

        public AuthController(
            IAuthService authService,
            IConfiguration configuration,
            IPasswordResetService passwordResetService,
            IUserRepository userRepository)
        {
            _authService = authService;
            _configuration = configuration;
            _passwordResetService = passwordResetService;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto);
                SetAuthCookie(result.Token);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during registration", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                SetAuthCookie(result.Token);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("authToken", new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Path = "/"
            });
            return NoContent();
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            // Always return 200 — don't reveal whether the email is registered
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user != null)
            {
                var token = _passwordResetService.GenerateToken(user.UserId);
                var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
                var resetLink = $"{frontendUrl}/reset-password?token={Uri.EscapeDataString(token)}";
                await _passwordResetService.SendResetEmailAsync(dto.Email, resetLink);
            }
            return Ok(new { message = "If that email is registered, a reset link has been sent." });
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var userId = _passwordResetService.ValidateToken(dto.Token);
            if (userId == null)
                return BadRequest(new { message = "Reset link is invalid or has expired." });

            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null)
                return BadRequest(new { message = "User not found." });

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            _passwordResetService.InvalidateToken(dto.Token);

            return Ok(new { message = "Password has been reset successfully." });
        }

        private void SetAuthCookie(string token)
        {
            var expiryMinutes = int.Parse(_configuration["JwtSettings:ExpiryMinutes"] ?? "60");

            Response.Cookies.Append("authToken", token, new CookieOptions
            {
                HttpOnly = true,
                // SameSite=None required: frontend (Vercel) and backend (Azure) are always on different origins
                SameSite = SameSiteMode.None,
                Secure = true,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes)
            });
        }
    }
}

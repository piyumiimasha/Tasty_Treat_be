using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        string GenerateJwtToken(User user);


    }
}

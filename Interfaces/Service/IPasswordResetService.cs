namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IPasswordResetService
    {
        string GenerateToken(int userId);
        int? ValidateToken(string token);
        void InvalidateToken(string token);
        Task SendResetEmailAsync(string email, string resetLink);
    }
}

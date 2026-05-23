using System.Collections.Concurrent;
using System.Net;
using System.Net.Mail;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PasswordResetService> _logger;

        // token → (userId, expiry) — singleton so state survives across requests
        private readonly ConcurrentDictionary<string, (int UserId, DateTime Expiry)> _tokens = new();

        public PasswordResetService(IConfiguration configuration, ILogger<PasswordResetService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateToken(int userId)
        {
            // Remove any existing outstanding token for this user
            foreach (var key in _tokens.Where(kv => kv.Value.UserId == userId).Select(kv => kv.Key).ToList())
                _tokens.TryRemove(key, out _);

            var raw = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var token = raw.Replace("/", "_").Replace("+", "-").TrimEnd('=');
            _tokens[token] = (userId, DateTime.UtcNow.AddMinutes(15));
            return token;
        }

        public int? ValidateToken(string token)
        {
            if (_tokens.TryGetValue(token, out var entry))
            {
                if (DateTime.UtcNow <= entry.Expiry)
                    return entry.UserId;
                _tokens.TryRemove(token, out _);
            }
            return null;
        }

        public void InvalidateToken(string token) => _tokens.TryRemove(token, out _);

        public async Task SendResetEmailAsync(string email, string resetLink)
        {
            var settings = _configuration.GetSection("EmailSettings");
            var smtpHost = settings["SmtpHost"];
            var smtpUser = settings["SmtpUser"];

            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUser))
            {
                // No SMTP configured — log the link so devs can test without email
                _logger.LogWarning("SMTP not configured. Password reset link for {Email}: {Link}", email, resetLink);
                return;
            }

            try
            {
                var fromEmail = settings["FromEmail"] ?? "noreply@tastytreat.com";
                var fromName = settings["FromName"] ?? "Tasty Treat";

                using var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = "Reset your Tasty Treat password",
                    IsBodyHtml = true,
                    Body = $@"<div style=""font-family:sans-serif;max-width:480px;margin:0 auto;padding:24px"">
  <h2 style=""color:#c0392b;margin-bottom:8px"">Reset your password</h2>
  <p style=""color:#555"">We received a request to reset the password for your Tasty Treat account associated with this email address.</p>
  <p style=""color:#555"">Click the button below to choose a new password. This link expires in <strong>15 minutes</strong>.</p>
  <a href=""{resetLink}"" style=""display:inline-block;padding:12px 28px;background:#c0392b;color:#fff;text-decoration:none;border-radius:8px;font-weight:600;margin:20px 0"">
    Reset Password
  </a>
  <p style=""color:#888;font-size:13px;margin-top:24px"">If you didn't request this, you can safely ignore this email. Your password will remain unchanged.</p>
  <hr style=""border:none;border-top:1px solid #eee;margin:24px 0""/>
  <p style=""color:#bbb;font-size:12px"">Tasty Treat &mdash; Artisan Cakes</p>
</div>"
                };
                message.To.Add(email);

#pragma warning disable SYSLIB0045
                using var client = new SmtpClient(smtpHost, int.Parse(settings["SmtpPort"] ?? "587"))
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(smtpUser, settings["SmtpPassword"])
                };
                await client.SendMailAsync(message);
#pragma warning restore SYSLIB0045
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send reset email to {Email}", email);
            }
        }
    }
}

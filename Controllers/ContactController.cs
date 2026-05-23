using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IConfiguration configuration, ILogger<ContactController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Send([FromBody] ContactDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Subject) ||
                string.IsNullOrWhiteSpace(dto.Message))
            {
                return BadRequest(new { message = "All fields are required." });
            }

            var settings = _configuration.GetSection("EmailSettings");
            var smtpHost = settings["SmtpHost"];
            var smtpUser = settings["SmtpUser"];

            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUser))
            {
                _logger.LogWarning("Contact form submission (SMTP not configured) — From: {Name} <{Email}>, Subject: {Subject}", dto.Name, dto.Email, dto.Subject);
                return Ok(new { message = "Message received." });
            }

            try
            {
                var fromEmail = settings["FromEmail"] ?? "noreply@tastytreat.com";
                var fromName = settings["FromName"] ?? "Tasty Treat";

                using var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = $"[Contact] {dto.Subject}",
                    IsBodyHtml = true,
                    Body = $@"<div style=""font-family:sans-serif;max-width:560px;margin:0 auto;padding:24px"">
  <h2 style=""color:#c0392b;margin-bottom:4px"">New Contact Message</h2>
  <p style=""color:#888;font-size:13px;margin-top:0"">via Tasty Treat website</p>
  <table style=""width:100%;border-collapse:collapse;margin:16px 0"">
    <tr><td style=""padding:8px 0;color:#555;font-size:13px;width:80px""><strong>Name</strong></td><td style=""padding:8px 0;color:#222;font-size:13px"">{System.Net.WebUtility.HtmlEncode(dto.Name)}</td></tr>
    <tr><td style=""padding:8px 0;color:#555;font-size:13px""><strong>Email</strong></td><td style=""padding:8px 0;font-size:13px""><a href=""mailto:{System.Net.WebUtility.HtmlEncode(dto.Email)}"" style=""color:#c0392b"">{System.Net.WebUtility.HtmlEncode(dto.Email)}</a></td></tr>
    <tr><td style=""padding:8px 0;color:#555;font-size:13px""><strong>Subject</strong></td><td style=""padding:8px 0;color:#222;font-size:13px"">{System.Net.WebUtility.HtmlEncode(dto.Subject)}</td></tr>
  </table>
  <div style=""background:#f9f9f9;border-left:3px solid #c0392b;padding:16px;border-radius:4px;margin-top:8px"">
    <p style=""margin:0;color:#333;font-size:14px;white-space:pre-wrap;line-height:1.6"">{System.Net.WebUtility.HtmlEncode(dto.Message)}</p>
  </div>
  <p style=""color:#bbb;font-size:12px;margin-top:24px"">Reply directly to this email to respond to {System.Net.WebUtility.HtmlEncode(dto.Name)}.</p>
</div>"
                };

                message.To.Add(smtpUser);
                message.ReplyToList.Add(new MailAddress(dto.Email, dto.Name));

#pragma warning disable SYSLIB0045
                using var client = new SmtpClient(smtpHost, int.Parse(settings["SmtpPort"] ?? "587"))
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(smtpUser, settings["SmtpPassword"])
                };
                await client.SendMailAsync(message);
#pragma warning restore SYSLIB0045

                return Ok(new { message = "Your message has been sent!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send contact email from {Email}", dto.Email);
                return StatusCode(500, new { message = "Failed to send message. Please try again." });
            }
        }
    }
}

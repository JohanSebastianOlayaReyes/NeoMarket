using Microsoft.AspNetCore.Mvc;
using Utilities.Mail;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestEmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public TestEmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-simple")]
        public async Task<IActionResult> SendSimpleEmail([FromBody] SimpleEmailRequest request)
        {
            try
            {
                var result = await _emailService.SendEmailAsync(
                    request.To,
                    request.Subject,
                    request.Body,
                    request.IsHtml);

                if (result)
                    return Ok(new { message = "Email enviado exitosamente" });
                else
                    return BadRequest(new { message = "Error al enviar email" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpPost("send-multiple")]
        public async Task<IActionResult> SendMultipleEmail([FromBody] MultipleEmailRequest request)
        {
            try
            {
                var result = await _emailService.SendEmailToManyAsync(
                    request.Recipients,
                    request.Subject,
                    request.Body,
                    request.IsHtml);

                if (result)
                    return Ok(new { message = "Emails enviados exitosamente" });
                else
                    return BadRequest(new { message = "Error al enviar emails" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpPost("send-with-copy")]
        public async Task<IActionResult> SendEmailWithCopy([FromBody] EmailWithCopyRequest request)
        {
            try
            {
                var result = await _emailService.SendEmailWithCopyAsync(
                    request.To,
                    request.Subject,
                    request.Body,
                    request.CC,
                    request.BCC,
                    request.IsHtml);

                if (result)
                    return Ok(new { message = "Email enviado exitosamente con copias" });
                else
                    return BadRequest(new { message = "Error al enviar email" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno: {ex.Message}" });
            }
        }
    }

    // DTOs para las peticiones
    public class SimpleEmailRequest
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; } = true;
    }

    public class MultipleEmailRequest
    {
        public List<string> Recipients { get; set; } = new();
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; } = true;
    }

    public class EmailWithCopyRequest
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public List<string>? CC { get; set; }
        public List<string>? BCC { get; set; }
        public bool IsHtml { get; set; } = true;
    }
}
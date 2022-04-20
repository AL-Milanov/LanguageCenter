using LanguageCenter.Core.Models.Email;
using LanguageCenter.Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LanguageCenter.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IEmailService _mailService;
        public MailController(IEmailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        [Route("send-email")]
        public IActionResult SendMessage(Contact msg)
        {

            if (_mailService.SendEmail(msg))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
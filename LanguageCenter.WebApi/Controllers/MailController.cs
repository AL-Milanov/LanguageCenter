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

        /// <summary>
        /// Sends message to owner email address
        /// </summary>
        /// <param name="msg">
        /// Object with parameters:
        /// 1.Name (required)
        /// 2.Email (required)
        /// 3.Phone number
        /// 4.Subject
        /// 5.Content
        /// </param>
        /// <returns>Doesnt return result.</returns>
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
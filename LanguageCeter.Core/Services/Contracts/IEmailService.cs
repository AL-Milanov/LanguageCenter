using LanguageCenter.Core.Models.Email;

namespace LanguageCenter.Core.Services.Contracts
{
    public interface IEmailService
    {
        bool SendEmail(Contact message);
    }
}

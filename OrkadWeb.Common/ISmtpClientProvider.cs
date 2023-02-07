using System.Net.Mail;

namespace OrkadWeb.Common
{
    public interface ISmtpClientProvider
    {
        SmtpClient GetSmtpClient();
    }
}

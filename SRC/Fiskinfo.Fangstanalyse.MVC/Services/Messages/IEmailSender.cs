using System.Threading.Tasks;

namespace SintefSecureBoilerplate.MVC.Services.Messages
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
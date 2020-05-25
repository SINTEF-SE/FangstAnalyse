using System.Threading.Tasks;

namespace SintefSecureBoilerplate.MVC.Services.Messages
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
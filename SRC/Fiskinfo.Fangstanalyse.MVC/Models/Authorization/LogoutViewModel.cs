using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SintefSecureBoilerplate.MVC.Models.Authorization
{
    public class LogoutViewModel
    {
        [BindNever]
        public string RequestId { get; set; }
    }
}
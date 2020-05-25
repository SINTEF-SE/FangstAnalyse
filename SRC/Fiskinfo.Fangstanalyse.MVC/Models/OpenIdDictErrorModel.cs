using System.ComponentModel.DataAnnotations;

namespace SintefSecureBoilerplate.MVC.Models
{
    public class OpenIdDictErrorModel
    {
        [Display(Name = "Error")]
        public string Error { get; set; }

        [Display(Name = "Description")]
        public string ErrorDescription { get; set; }
    }
}
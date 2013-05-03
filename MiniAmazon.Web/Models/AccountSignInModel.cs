using System.ComponentModel.DataAnnotations;

namespace MiniAmazon.Web.Models
{
    public class AccountSignInModel
    {
        [Required(ErrorMessage = "Enter a Email Adress")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Enter a Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MiniAmazon.Web.Models
{
    public class AccountInputModel
    {
        public long Id { get; set; }
        
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter a Email Adress")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        [Remote("ValidatedEmail", "Account",ErrorMessage = "There is a account already created with that email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter a Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int Age { get; set; }
        
        public string Genre { get; set; }
        public bool Admin { get; set; }
        public bool Active { get; set; }
    }
}
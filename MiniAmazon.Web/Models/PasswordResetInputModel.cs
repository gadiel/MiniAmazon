using System.ComponentModel.DataAnnotations;

namespace MiniAmazon.Web.Models
{
    public class PasswordResetInputModel
    {
        

        private string email { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password, ErrorMessage = "Error.")]
        
        public string Password { get; set; }
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password, ErrorMessage = "Error.")]
        
        public string PasswordTwo { get; set; }

        public PasswordResetInputModel() { }
        public PasswordResetInputModel(string email)
        {
            this.email = email;
        }

        public string emailObtainer()
        {
            return email;
        }
    }
}
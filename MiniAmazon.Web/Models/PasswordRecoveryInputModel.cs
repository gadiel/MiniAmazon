using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniAmazon.Web.Models
{
    public class PasswordRecoveryInputModel
    {
        public long Id { get; set; }
        
        [Required(ErrorMessage = "Enter a Email Adress")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        [Remote("EmailExist", "Account", ErrorMessage = "There is no account already created with that email")]
        public string Email { get; set; }
    }
}
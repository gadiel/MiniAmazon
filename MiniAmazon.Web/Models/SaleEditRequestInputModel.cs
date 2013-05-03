using System.ComponentModel.DataAnnotations;

namespace MiniAmazon.Web.Models
{
    public class SaleEditRequestInputModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Required")]
        public int Amount { get; set; }
        
        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Currency, ErrorMessage = "Invalid Input")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Required")]
        public int CategoryId { get; set; }
        
        [Required(ErrorMessage = "Required")]
        public int SaleId { get; set; }
        
        public string MessageToAdmin { get; set; }
         public string YoutubeLink { get; set; }
        //[Required(ErrorMessage = "Required")]
       // [DataType(DataType.ImageUrl, ErrorMessage = "Invalid")]
        public  string Photo { get; set; }
        

    }
}
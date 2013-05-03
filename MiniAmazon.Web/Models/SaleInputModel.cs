using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiniAmazon.Domain.Entities;
using MiniAmazon.Web.Controllers;


namespace MiniAmazon.Web.Models
{
    public class SaleInputModel
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
        //[Required(ErrorMessage = "Required")]
        //[DataType(DataType.Url, ErrorMessage = "Invalid")]
        public string YoutubeLink { get; set; }
        //[Required(ErrorMessage = "Required")]
        //[DataType(DataType.ImageUrl, ErrorMessage = "Invalid")]
        public  string Photo { get; set; }
        [Required(ErrorMessage = "Required")]
        public int AccountId { get; set; }
        [Required(ErrorMessage = "Required")]

        public int CategoryId { get; set; }

        public SelectList Categories { get; set; }

        public bool Active { get; set; }

        public SaleInputModel(){}

        public SaleInputModel(IEnumerable<Category> categories)
        {
            CategoryInitializer(categories);
            
        }

        public void CategoryInitializer(IEnumerable<Category> categories)
        {
            Categories = new SelectList(categories.OrderBy(x => x.Name), "Id", "Name", CategoryId);
        }


        
    }
}
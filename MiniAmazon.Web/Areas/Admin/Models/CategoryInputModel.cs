using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MiniAmazon.Domain.Entities;

namespace MiniAmazon.Web.Models
{
    public class CategoryInputModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Name { get; set; }

        
        public bool Active { get; set; }
    }
}
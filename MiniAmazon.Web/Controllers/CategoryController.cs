using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MiniAmazon.Domain;
using MiniAmazon.Domain.Entities;
using MiniAmazon.Web.Infrastructure;
using MiniAmazon.Web.Models;

namespace MiniAmazon.Web.Controllers
{
    public class CategoryController : BootstrapBaseController
    {
        private readonly IRepository _repository;
        private readonly IMappingEngine _mappingEngine;

        public CategoryController(IRepository repository, IMappingEngine mappingEngine)
        {
            _repository = repository;
            _mappingEngine = mappingEngine;
        }

        public ActionResult Products(string id)
        {
            var model = _repository.Query<Sale>(x => x.Category.Name == id);
            
            ViewBag.id = id;
            if(!model.Equals(null))
            {
                ViewBag.CategoryName = model.First().Category.Name;
            }
                
            return View(new CategoryGeneralModel(model, _repository));
        }

    }
}

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
    public class CategoriesController : BootstrapBaseController
    {
        private readonly IRepository _repository;
        private readonly IMappingEngine _mappingEngine;

        public CategoriesController(IRepository repository, IMappingEngine mappingEngine)
        {
            _repository = repository;
            _mappingEngine = mappingEngine;
        }

        public ActionResult Index()
        {
            IEnumerable<Category> jmz = _repository.Query<Category>(x => x == x); 
            return View(new CategoryGeneralModel(jmz,_repository));
        }

       // public ActionResult Add()
       // {
       //     IEnumerable<Account> jmz = _repository.Query<Account>(x => x == x);
       //     return View(jmz);
       // }

        public ActionResult Create()
        {
            return View(new CategoryInputModel());
        }

        [HttpPost]
        public ActionResult Create(CategoryInputModel categoryInputModel)
        {
            var category = Mapper.Map<CategoryInputModel, Category>(categoryInputModel);

            category.Created = System.DateTime.Now;
            _repository.Create(category);
            
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var model = _repository.First<Category>(x => x.Id == id);
            model.Active = false;
            _repository.Update(model);
            Information("The Category "+model.Name+" was disabled. Remember you cannot delete elements.");
            
            return RedirectToAction("index");
        }
        public ActionResult Edit(int id)
        {
            var cat = _repository.GetById<Category>(id);
            var model = Mapper.Map<Category, CategoryInputModel>(cat);

            return View("Create", model);
        }
        [HttpPost]
        public ActionResult Edit(CategoryInputModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var category = _repository.GetById<Category>(id);
                var tmpDate = category.Created;
                category = Mapper.Map<CategoryInputModel, Category>(model);
                category.Created = tmpDate;

                _repository.Update(category);
                
                Success("The model was updated!");
                return RedirectToAction("index");
            }
            return View("Create", model);
        }

        public ActionResult Details(int id)
        {
            var model = _repository.GetById<Category>(id);
            return View(model);
        }

    }
}

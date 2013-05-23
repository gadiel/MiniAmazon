using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using MiniAmazon.Domain;
using MiniAmazon.Domain.Entities;
using MiniAmazon.Web.Controllers;
using MiniAmazon.Web.Infrastructure;
using MiniAmazon.Web.Models;

namespace MiniAmazon.Web.Areas.Admin.Controllers
{
    public class SalesController : BootstrapBaseController
    {
        private readonly IRepository _repository;
        private readonly IMappingEngine _mappingEngine;

        public SalesController(IRepository repository, IMappingEngine mappingEngine)
        {
            _repository = repository;
            _mappingEngine = mappingEngine;
        }

        public ActionResult Index()
        {
            IEnumerable<Sale> jmz = _repository.Query<Sale>(x => x == x); 
            return View(new CategoryGeneralModel(jmz,_repository));
        }

        

       // public ActionResult Add()
       // {
       //     IEnumerable<Account> jmz = _repository.Query<Account>(x => x == x);
       //     return View(jmz);
       // }
        
        public ActionResult Create()
        {
            return View(new SaleInputModel(_repository.Query<Category>(x => x == x)));
        }

        [HttpPost]
        public ActionResult Create(SaleInputModel saleInputModel)
        {
            var account = _repository.First<Account>(x => x.Id == saleInputModel.AccountId);
            var category = _repository.First<Category>(x => x.Id == saleInputModel.CategoryId);
            var sale = Mapper.Map<SaleInputModel, Sale>(saleInputModel);

            sale.CreateDateTime = DateTime.Now;
            sale.Category = category;
            _repository.Create(sale);
            
            account.AddSale(sale);
            _repository.Update(account);
            
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var model = _repository.First<Sale>(x => x.Id == id);
            model.Active = false;
            _repository.Update(model);
            Information("The Sale " + model.Description + " was disabled. Remember you cannot delete elements.");

            return RedirectToAction("index");
        }
        public ActionResult Edit(int id)
        {
            var sale = _repository.GetById<Sale>(id);
            var model = Mapper.Map<Sale, SaleInputModel>(sale);
            model.CategoryInitializer(_repository.Query<Category>(x => x == x));

            return View("Create", model);
        }
        [HttpPost]
        public ActionResult Edit(SaleInputModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var sale = _repository.GetById<Sale>(model.Id);

                //canchada
                var dateTime = sale.CreateDateTime;
                //fin chachada

                sale = Mapper.Map<SaleInputModel, Sale>(model);

                var category = _repository.First<Category>(x => x.Id == model.CategoryId);
                sale.Category = category;

                sale.CreateDateTime = dateTime;
                
                _repository.Update(sale);
                var account = _repository.First<Account>(x => x.Id == model.AccountId);
                
                account.AddSale(sale);
                _repository.Update(account);

                Success("The model was updated!");
                return RedirectToAction("index");
            }
            return View("Create", model);
        }

        public ActionResult Details(int id)
        {
            var model = _repository.GetById<Sale>(id);
            return View(model);
        }

        public ActionResult EditRequest(int id)
        {

            var sale = _repository.GetById<Sale>(id);
            var saleEdit = Mapper.Map<Sale, SaleEditRequest>(sale);
            var model = Mapper.Map<SaleEditRequest, SaleEditRequestInputModel>(saleEdit);

            return View("EditRequest", model);
        }
        [HttpPost]
        public ActionResult EditRequest(SaleEditRequestInputModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var sale = _repository.First<Sale>(x => x.Id == model.SaleId);
                var category = _repository.First<Category>(x => x.Id == model.CategoryId);
                var editRequest = Mapper.Map<SaleEditRequestInputModel, SaleEditRequest>(model);
                editRequest.CreateDateTime = sale.CreateDateTime;
                editRequest.EditRequestTime = DateTime.Now;
                editRequest.Reviewed = false;
                editRequest.OriginalSale = sale;
                editRequest.Category = category;

                _repository.Create(editRequest);

                //category.AddSaleEdit(editRequest);

                //account.AddSaleEdit(editRequest);

                //sale.AddSaleEdit(editRequest);
                //_repository.Update(sale);

                Success("The edit request was send!");
                return RedirectToAction("index");
            }
            return View("EditRequest", model);
        }

        public ActionResult EditAprove(int id)
        {
            var sale = _repository.GetById<SaleEditRequest>(id);
            var model = _mappingEngine.Map<SaleEditRequest, SaleEditRequestInputModel>(sale);
            

            return View("EditCreate", model);
        }
        [HttpPost]
        public ActionResult EditAprove(SaleEditRequestInputModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var sale = _repository.GetById<SaleEditRequest>(model.Id);
                var sale2 = _repository.GetById<Sale>(model.SaleId);
                sale.OriginalSale = sale2;

                _repository.Update(sale);
                

                Success("The model was updated!");
                return RedirectToAction("index");
            }
            return View("Create", model);
        }
    }
}

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
    public class SaleController : BootstrapBaseController
    {
        private readonly IRepository _repository;
        private readonly IMappingEngine _mappingEngine;

        public SaleController(IRepository repository, IMappingEngine mappingEngine)
        {
            _repository = repository;
            _mappingEngine = mappingEngine;
        }

        public ActionResult Index()
        {
            var account = _repository.First<Account>(x => x.Email == User.Identity.Name); 

            return View(new CategoryGeneralModel(account.Sales,_repository));
        }
        
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
            var account = _repository.First<Account>(x => x.Email == User.Identity.Name);
            if(account.Sales.First(x => x == model)!=null)
            {
                model.Active = false;
                _repository.Update(model);
                Information("The Sale " + model.Description + " was disabled. Remember you cannot delete elements.");

                return RedirectToAction("index");
            }
            Error("The Sale " + model.Description + " was NOT disabled.");

            return RedirectToAction("index");
            
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
            return View("EditCreate", model);
        }

        public ActionResult GoodOffer(int id)
        {
            var sale = _repository.First<Sale>(x =>  x.Id == id);
            var account = _repository.First<Account>(x => x.Email == User.Identity.Name);
            if(_repository.First<GoodOffer>(x => x.Sale==sale&&x.Account==account)==null)
            {
                var offer = new GoodOffer {Account = account, Sale = sale};
                _repository.Create(offer);
                Success("Marked as a good offer");
            }
            else
            {
                Error("Already marked as a good offer");
            }
            return View("Details", sale);

        }

    }
}


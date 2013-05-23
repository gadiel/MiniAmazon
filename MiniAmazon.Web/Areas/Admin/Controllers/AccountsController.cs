using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using MiniAmazon.Domain;
using MiniAmazon.Domain.Entities;
using MiniAmazon.Web.Controllers;
using MiniAmazon.Web.Infrastructure;
using MiniAmazon.Web.Models;
using System.Security.Cryptography;

namespace MiniAmazon.Web.Areas.Admin.Controllers
{
    public class AccountsController : BootstrapBaseController
    {
        private readonly IRepository _repository;
        private readonly IMappingEngine _mappingEngine;

        public AccountsController(IRepository repository, IMappingEngine mappingEngine)
        {
            _repository = repository;
            _mappingEngine = mappingEngine;
        }


        [Authorize]
        public ActionResult Index()
        {
            var model = _repository.Query<Account>(x => true );
            return View(new CategoryGeneralModel(model, _repository));
        }

        public ActionResult Create()
        {
            return View(new AccountInputModel());
        }

        [HttpPost]
        public ActionResult Create(AccountInputModel accountInputModel)
        {
            var account = Mapper.Map<AccountInputModel, Account>(accountInputModel);
            

            _repository.Create(account);
            
            return RedirectToAction("Index","Sale");
        }

        public ActionResult Delete(int id)
        {
            var model = _repository.First<Account>(x => x.Id == id);
            model.Active = false;
            _repository.Update(model);
            Information("The Account " + model.Name + " was disabled. Remember you cannot delete elements.");

            return RedirectToAction("index");
        }
        public ActionResult Edit(int id)
        {
            var cat = _repository.GetById<Account>(id);
            var model = Mapper.Map<Account, AccountInputModel>(cat);

            return View("Create", model);
        }
        [HttpPost]
        public ActionResult Edit(AccountInputModel model, int id)
        {
            if (ModelState.IsValid)
            {
                
                var account = _repository.GetById<Account>(model.Id);
                var tempSale = account.Sales;
                account = _mappingEngine.Map<AccountInputModel, Account>(model);
                account.Sales = tempSale;
                _repository.Update(account);

                Success("The model was updated!");
                return RedirectToAction("index");
            }
            return View("Create", model);
        }

        public ActionResult Details(int id)
        {
            var model = _repository.GetById<Account>(id);
            return View(model);
        }

    }
}
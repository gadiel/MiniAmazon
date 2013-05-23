using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MiniAmazon.Domain;
using MiniAmazon.Domain.Entities;
using MiniAmazon.Web.Infrastructure;

namespace MiniAmazon.Web.Controllers
{
    public class UserController : BootstrapBaseController
    {
        //
        // GET: /User/

        public ActionResult Index(string id)
        {
            var model = _repository.First<Account>(x => x.Email == id);
            if(User.Identity.Name==model.Email)
            {
                return RedirectToAction("Index", "Account");
            }
            if(model.Equals(null))
            {
                Error("Not found");
                return RedirectToAction("Index", "Account");
            }
            
            return View(new UserGeneralModel(model));
        }

        public ActionResult Follow(string id)
        {
            var model = _repository.First<Account>(x => x.Email == id);
            if (User.Identity.Name == model.Email)
            {
                return RedirectToAction("Index", "Account");
            }
            if (model.Equals(null))
            {
                Error("Not found");
                return RedirectToAction("Index", "Account");
            }
            if (model.Followers==null)
            {
                model.Followers = User.Identity.Name;
            }
            else
            {
                model.Followers.Insert(model.Followers.Length, "," + User.Identity.Name);
            }
            
            _repository.Update(model);

            return View("Index",model);
        }
        
        private readonly IRepository _repository;
        private readonly IMappingEngine _mappingEngine;

        public UserController(IRepository repository, IMappingEngine mappingEngine)
        {
            _repository = repository;
            _mappingEngine = mappingEngine;
        }

    }
}

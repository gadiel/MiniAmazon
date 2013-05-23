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
    public class SearchController : BootstrapBaseController
    {
        private readonly IRepository _repository;
        private readonly IMappingEngine _mappingEngine;

        public SearchController(IRepository repository, IMappingEngine mappingEngine)
        {
            _repository = repository;
            _mappingEngine = mappingEngine;
        }
        //
        // GET: /Search/

        public ActionResult Index(string id = "")
        {
            var model = _repository.Query<Sale>(x => x.Description.Contains(id) || x.Category.Name.Contains(id) || x.Title.Contains(id));
            return View("Index", new CategoryGeneralModel(model, _repository));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Machine.Specifications;
using MiniAmazon.Domain;
using MiniAmazon.Web.Controllers;
using Moq;

namespace MiniAmazon.Web.Specs
{
    public class given_an_account_controller_context
    {
        private Establish context = () =>
            {
                MockRepository = new Mock<IRepository>();
                MockMappingEngine = new Mock<IMappingEngine>();

                AccountController = new AccountController(MockRepository.Object, MockMappingEngine.Object);

            };

        protected static Mock<IRepository> MockRepository;
        protected static Mock<IMappingEngine> MockMappingEngine;
        protected static AccountController AccountController;
    }
}

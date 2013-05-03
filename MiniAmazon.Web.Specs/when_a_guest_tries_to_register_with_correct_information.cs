using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Machine.Specifications;
using Machine.Specifications.Mvc;
using MiniAmazon.Domain.Entities;
using MiniAmazon.Web.Models;

namespace MiniAmazon.Web.Specs
{
    public class when_a_guest_tries_to_register_with_correct_information : given_an_account_controller_context
    {
        private static AccountInputModel _accountInputModel;
        private static ActionResult _result;
        private static Account _account;
        private static Account _accountMapped;

        private Establish context = () =>
            {
                _accountInputModel = new AccountInputModel
                    {
                        Email = "camilo@me.com",
                        Name = "Camilo",
                        Password = "pass1",
                        Age = 26,
                        Genre = "M"
                    };

                _account = new Account();

                _accountMapped = new Account
                    {
                        Email = "camilo@me.com",
                        Name = "Camilo",
                        Password = "pass1",
                        Age = 26,
                        Genre = "M"
                    };

              /*  MockRepository.Setup(
                    x => x.Create(Moq.It.Is<Expression<Func<Account, bool>>>(y => y.Compile().Invoke(_account))))
                    .Returns(_account);

                MockMappingEngine
                    .Setup(x => x.Map<AccountInputModel, Account>(_accountInputModel))
                    .Returns(_account);*/
            };

        private Because of = () => { _result = AccountController.Create(_accountInputModel); };
        private It should_map_account_input_model_to_account_model = () => { _account.ShouldBeLike(_accountMapped); };

        private It should_redirect_to_dashboard_action =
            () => { _result.ShouldBeARedirectToRoute().And().ActionName().ShouldEqual("Dashboard"); };

        private It should_redirect_to_sales_controller =
            () => { _result.ShouldBeARedirectToRoute().And().ControllerName().ShouldEqual("Sales"); };
    }
}
using System.Web.Mvc;
using Machine.Specifications;
using Machine.Specifications.Mvc;
using MiniAmazon.Web.Models;

namespace MiniAmazon.Web.Specs
{
    public class when_a_user_signins_with_correct_values : given_an_account_controller_context
    {
        private Establish context = () =>
            {
                _accountSignInModel = new AccountSignInModel
                    {
                        Email = "camilo@me.com",
                        Password = "pass123",
                        RememberMe = true
                    };
            };

        private Because of = () => {
                                       _result = AccountController.SignIn(_accountSignInModel);
        };

        private It should_redirect_to_action_index = () =>
            {
                _result.ShouldBeARedirectToRoute().And().ActionName().ShouldEqual("Index");
            };

        private static AccountSignInModel _accountSignInModel;
        private static ActionResult _result;
    }
}
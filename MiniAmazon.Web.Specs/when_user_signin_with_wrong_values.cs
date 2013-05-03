using System.Web.Mvc;
using Machine.Specifications;
using Machine.Specifications.Mvc;
using MiniAmazon.Web.Models;

namespace MiniAmazon.Web.Specs
{
    public class when_user_signin_with_wrong_values : given_an_account_controller_context
    {
        private Establish context = () =>
            {
                _accountSignInModel = new AccountSignInModel
                    {
                        Email = "camilo1@me.com",
                        Password = "passssd21123",
                        RememberMe = false
                    };



            };

        private Because of = () => {
                                       _result = AccountController.SignIn(_accountSignInModel);
        };

        private It should_return_to_the_same_view = () =>
            {
                _result.ShouldBeAView();
            };

        private static AccountSignInModel _accountSignInModel;
        private static ActionResult _result;
    }
}
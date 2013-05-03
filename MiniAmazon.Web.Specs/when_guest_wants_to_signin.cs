using System.Web.Mvc;
using Machine.Specifications;
using Machine.Specifications.Mvc;
using MiniAmazon.Web.Models;

namespace MiniAmazon.Web.Specs
{
    public class when_guest_wants_to_signin : given_an_account_controller_context
    {
        private Establish context = () =>
            {
                
            };

        private Because of = () => {
                                       _result = AccountController.SignIn();
        };

        private It should_return_a_view_with_a_signin_model = () =>
            {
                _result.ShouldBeAView().And().ShouldHaveModelOfType<AccountSignInModel>();
            };

        private static ActionResult _result;
    }
}
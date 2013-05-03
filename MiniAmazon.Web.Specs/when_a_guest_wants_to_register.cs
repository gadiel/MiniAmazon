using System.Web.Mvc;
using Machine.Specifications;
using Machine.Specifications.Mvc;
using MiniAmazon.Web.Models;

namespace MiniAmazon.Web.Specs
{
    public class when_a_guest_wants_to_register : given_an_account_controller_context
    {
        private Establish context = () =>
            {
                
            };

        private Because of = () => {
                                       _result = AccountController.Create();
        };

        private It should_return_the_create_account_view_with_a_account_input_model = () =>
            {
                _result.ShouldBeAView().And().ShouldHaveModelOfType<AccountInputModel>();
            };

      

        private static ActionResult _result;
    }
}
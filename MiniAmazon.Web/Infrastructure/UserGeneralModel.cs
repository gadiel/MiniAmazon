using System.Collections.Generic;
using MiniAmazon.Domain.Entities;

namespace MiniAmazon.Web.Infrastructure
{
    public class UserGeneralModel
    {
        public UserGeneralModel(Account account)
        {
            DefineAccount(account, Account.Sales);
        }


        public void DefineAccount(Account account, IEnumerable<Sale> _Sales)
        {
            Account = account;
            SetSaleData(_Sales);
        }

        private void SetSaleData(IEnumerable<Sale> _Sales)
        {
            Sales = _Sales;
        }

        public Account Account { get; private set; }
        public IEnumerable<Sale> Sales { get; private set; }

    }
}
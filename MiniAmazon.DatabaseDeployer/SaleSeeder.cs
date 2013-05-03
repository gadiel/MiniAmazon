using System.Linq;
using DomainDrivenDatabaseDeployer;
using FizzWare.NBuilder;
using MiniAmazon.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace MiniAmazon.DatabaseDeployer
{
    public class SaleSeeder : IDataSeeder
    {
        private readonly ISession _session;

        public SaleSeeder(ISession session)
        {
            _session = session;
        }

        public void Seed()
        {
            var account = _session.Query<Account>().First(x => x.Email == "gadi@me.com");
            var sale = Builder<Sale>.CreateNew().Build();
            _session.Save(sale);
            account.AddSale(sale);
            _session.Update(account);
           
            

        }
    }
}
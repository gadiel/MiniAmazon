using System.Linq;
using DomainDrivenDatabaseDeployer;
using FizzWare.NBuilder;
using MiniAmazon.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace MiniAmazon.DatabaseDeployer
{
    public class SaleEditSeeder : IDataSeeder
    {
        private readonly ISession _session;

        public SaleEditSeeder(ISession session)
        {
            _session = session;
        }

        public void Seed()
        {
            
            var saleEdit = Builder<SaleEditRequest>.CreateNew().Build();
            _session.Save(saleEdit);
            
            


        }
    }
}
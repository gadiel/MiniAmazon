using DomainDrivenDatabaseDeployer;
using FizzWare.NBuilder;
using MiniAmazon.Domain.Entities;
using NHibernate;

namespace MiniAmazon.DatabaseDeployer
{
    public class GoodOfferSeeder : IDataSeeder
    {
        private readonly ISession _session;

        public GoodOfferSeeder(ISession session)
        {
            _session = session;
        }

        public void Seed()
        {
            var offer = Builder<GoodOffer>.CreateNew().Build();

            _session.Save(offer);
        }
    }
}
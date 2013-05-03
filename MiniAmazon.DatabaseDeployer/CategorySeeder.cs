using DomainDrivenDatabaseDeployer;
using FizzWare.NBuilder;
using MiniAmazon.Domain.Entities;
using NHibernate;

namespace MiniAmazon.DatabaseDeployer
{
    public class CategorySeeder : IDataSeeder
    {
        private readonly ISession _session;

        public CategorySeeder(ISession session)
        {
            _session = session;
        }

        public void Seed()
        {
            var category = Builder<Category>.CreateNew().Build();
            _session.Save(category);
        }
    }
}
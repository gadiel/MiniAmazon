using DomainDrivenDatabaseDeployer;
using FizzWare.NBuilder;
using MiniAmazon.Domain.Entities;
using NHibernate;

namespace MiniAmazon.DatabaseDeployer
{
    public class PasswordRecoverySeeder : IDataSeeder
    {
        private readonly ISession _session;

        public PasswordRecoverySeeder(ISession session)
        {
            _session = session;
        }

        public void Seed()
        {
            var paswrecovery = Builder<PasswordRecovery>.CreateNew().Build();
            _session.Save(paswrecovery);
        }
    }
}
using DomainDrivenDatabaseDeployer;
using MiniAmazon.Domain.Entities;
using NHibernate;

namespace MiniAmazon.DatabaseDeployer
{
    public class AccountSeeder : IDataSeeder
    {
        private readonly ISession _session;

        public AccountSeeder(ISession session)
        {
            _session = session;
        }

        public void Seed()
        {
            var account = new Account
                {
                    Name = "Gadiel Ortez Velasquez",
                    Email = "gadi@me.com",
                    Password = "pass123",
                    Genre = "Male",
                    Age = 23,
                    Admin = true,
                    Active = true

                };

            _session.Save(account);
        }
    }
}
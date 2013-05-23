using System.Linq;
using DomainDrivenDatabaseDeployer;
using MiniAmazon.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

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

            var role1 = _session.Query<Role>().First(x => x.Name == "Admin");
            var role2 = _session.Query<Role>().First(x => x.Name == "User");

            var account = new Account{
                Name = "Gadiel Ortez Velasquez",
                Email = "gadi@me.com",
                Password = "pass123",
                Genre = "Male",
                Age = 23,
                Active = true,
                Role = role1
            };

            _session.Save(account);


            var account2 = new Account
            {
                Name = "Jimmmeh",
                Email = "lel@me.com",
                Password = "pass123",
                Genre = "Male",
                Age = 22,
                Active = true

            };
            account2.Role = role2;
            _session.Save(account2);

        }
    }
}
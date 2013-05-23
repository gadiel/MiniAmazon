using DomainDrivenDatabaseDeployer;
using MiniAmazon.Domain.Entities;
using NHibernate;

namespace MiniAmazon.DatabaseDeployer
{
    public class RoleSeeder : IDataSeeder
    {
        private readonly ISession _session;

        public RoleSeeder(ISession session)
        {
            _session = session;
        }

        public void Seed()
        {
            var role = new Role
                           {
                               Name = "Admin"
                           };

            var role2 = new Role
                            {
                                Name = "User"
                            };

            _session.Save(role);
            _session.Save(role2);
        }
    }
}
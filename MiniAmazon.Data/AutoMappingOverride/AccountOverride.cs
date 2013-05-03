using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;
using MiniAmazon.Domain.Entities;

namespace MiniAmazon.Data.AutoMappingOverride
{
    internal class AccountOverride : IAutoMappingOverride<Account>
    {
        public void Override(AutoMapping<Account> mapping)
        {
           /* mapping.HasMany(x => x.Referrals)
                .Inverse()
                .Access.CamelCaseField(Prefix.Underscore);*/
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace MiniAmazon.Domain.Entities
{
    public class Account : IEntity
    {
        private IList<Sale> _sales = new List<Sale>();

        public virtual Role Role { get; set; }

        public virtual string Name { get; set; }

        public virtual string Email { get; set; }

        public virtual string Followers { get; set; }

        public virtual string Password { get; set; }

        public virtual int Age { get; set; }

        public virtual string Genre { get; set; }


        public virtual bool Active { get; set; }

        public virtual IEnumerable<Sale> Sales
        {
            get { return _sales; }
            set { _sales = (IList<Sale>) value; }
        }

        
        #region IEntity Members

        public virtual long Id { get; set; }

        #endregion

        

        public virtual void AddSale(Sale sale)
        {
            if (!_sales.Contains(sale))
            {
                _sales.Add(sale);
            }
        }

        
    }

}
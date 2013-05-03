using System;

namespace MiniAmazon.Domain.Entities
{
    public class SaleEditRequest : IEntity
    {

        #region IEntity Members

        public virtual long Id { get; set; }

        #endregion

        public virtual DateTime EditRequestTime { get; set; }

        public virtual DateTime CreateDateTime { get; set; }

        public virtual string Title { get; set; }

        public virtual int Amount { get; set; }

        public virtual Category Category { get; set; }

        public virtual Sale OriginalSale { get; set; }

        public virtual string Description { get; set; }

        public virtual decimal Price { get; set; }

        public virtual string MessageToAdmin { get; set; }

        public virtual bool Reviewed { get; set; }

    }
}
using System;
using System.Collections.Generic;

namespace MiniAmazon.Domain.Entities
{
    public class Sale : IEntity
    {

        #region IEntity Members

        public virtual long Id { get; set; }

        #endregion

        public virtual DateTime CreateDateTime { get; set; }

        public virtual string Title { get; set; }

        public virtual int Amount { get; set; }

        public virtual Category Category { get; set; }

        public virtual string Description { get; set; }

        public virtual decimal Price { get; set; }

        public virtual string YoutubeLink { get; set; }

        public virtual string Photo { get; set; }

        public virtual bool Active { get; set; }

        

    }
}
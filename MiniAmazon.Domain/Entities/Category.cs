using System;
using System.Collections.Generic;

namespace MiniAmazon.Domain.Entities
{
    public class Category : IEntity
    {

        #region IEntity Members

        public virtual long Id { get; set; }

        #endregion

        public virtual string Name { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual bool Active { get; set; }

    }
}
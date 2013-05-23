using System;

namespace MiniAmazon.Domain.Entities
{
    public class PasswordRecovery : IEntity
    {
        public virtual string Token { get; set; }
        public virtual Account Account { get; set; }
        public virtual bool Used { get; set; }
        public virtual DateTime Created { get; set; }

        
        #region IEntity Members

        public virtual long Id { get; set; }

        #endregion

        
    }
}
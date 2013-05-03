using System;

namespace MiniAmazon.Domain.Entities
{
    public class PasswordRecovery : IEntity
    {
        public virtual string HashToken { get; set; }
        public virtual string HashKey { get; set; }
        public virtual string Iv { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual bool Used { get; set; }

        
        #region IEntity Members

        public virtual long Id { get; set; }

        #endregion

        
    }
}
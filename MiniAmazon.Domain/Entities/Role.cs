namespace MiniAmazon.Domain.Entities
{
    public class Role : IEntity
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }
    }
}
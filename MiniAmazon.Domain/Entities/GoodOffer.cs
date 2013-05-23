namespace MiniAmazon.Domain.Entities
{
    public class GoodOffer : IEntity
    {
        public virtual long Id { get; set; }

        public virtual Sale Sale { get; set; }

        public virtual Account Account { get; set; }
    }
}
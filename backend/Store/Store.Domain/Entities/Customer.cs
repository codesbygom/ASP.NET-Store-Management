using Store.Domain.Entities.Base;

namespace Store.Domain.Entities
{
    public class Customer : UserBase
    {
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

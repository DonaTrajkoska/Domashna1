using System;

namespace EShop.Domain.DomainModels
{
    public class BiletInOrder:BaseEntity
    {
        public Guid BiletId { get; set; }
        public Billet SelectedBilet { get; set; }
        public Guid OrderId { get; set; }
        public Order UserOrder { get; set; }
        public int Quantity { get; set; }   

    }
}

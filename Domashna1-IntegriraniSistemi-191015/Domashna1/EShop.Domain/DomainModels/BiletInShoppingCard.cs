using System;

namespace EShop.Domain.DomainModels
{
    public class BiletInShoppingCard :BaseEntity
    {
        public Guid BiletId { get; set; }
        public Billet Bilet { get; set; }
        public Guid ShoppingCartId { get; set; }
        public ShoppingCard ShoppingCard { get; set; }
        public int Quantity { get; set; }
    }
}

using EShop.Domain.DomainModels;


using System.Collections.Generic;

namespace EShop.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<BiletInShoppingCard> BiletInShoppingCards { get; set; }
        public double TotalPrice { get; set; }
    }
}

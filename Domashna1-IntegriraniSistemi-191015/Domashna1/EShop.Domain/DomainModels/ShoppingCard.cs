using EShop.Web.Models.Identity;
using System;
using System.Collections.Generic;

namespace EShop.Domain.DomainModels
{
    public class ShoppingCard :BaseEntity
    {

        public virtual ICollection<BiletInShoppingCard> BiletInShoppingCards { get; set; }
        public virtual EShopApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }
        public ShoppingCard() { }

    }
}

using EShop.Web.Models.Identity;
using System;
using System.Collections.Generic;

namespace EShop.Domain.DomainModels
{
    public class Order:BaseEntity
    {
        public string UserId { get; set; }
        public EShopApplicationUser User { get; set; }
        public virtual ICollection<BiletInOrder> Bilets { get; set; }
    }
}

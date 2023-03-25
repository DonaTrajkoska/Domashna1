using System;
using System.Collections.Generic;

namespace EShopAdminApplication.Models
{
    public class Order 
    {
        public Guid Id { get; set; } 
        public string UserId { get; set; }
        public EShopApplicationUser User { get; set; }
        public  ICollection<BiletInOrder> Bilets { get; set; }
    }
}

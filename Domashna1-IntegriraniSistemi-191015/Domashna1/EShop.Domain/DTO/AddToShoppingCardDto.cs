
using EShop.Domain.DomainModels;
using System;

namespace EShop.Domain.DTO
{
    public class AddToShoppingCardDto
    {
        public Billet SelectedProduct { get; set; }
        public Guid SelectedProductId { get; set; }
        public int Quantity { get; set; }
    }
}

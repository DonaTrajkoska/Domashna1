using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Services.Interface
{
    public interface IProductService
    {
        List<Billet> GetAllProducts();
        Billet GetDetailsForProduct(Guid? id);
        void CreateNewProduct(Billet p);
        void UpdeteExistingProduct(Billet p);
        AddToShoppingCardDto GetShoppingCartInfo(Guid? id);
        void DeleteProduct(Guid id);
        bool AddToShoppingCart(AddToShoppingCardDto item, string userID);
    }
}

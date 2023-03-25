using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Repository.Interface;
using EShop.Services.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShop.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IRepository<Billet> _productRepository;
        private readonly IRepository<BiletInShoppingCard> _productInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        public ProductService(IRepository<Billet> productRepository, ILogger<ProductService> logger, IUserRepository userRepository, IRepository<BiletInShoppingCard> productInShoppingCartRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
            _logger = logger;
        }

        public bool AddToShoppingCart(AddToShoppingCardDto item, string userID)
        {
            var user = this._userRepository.Get(userID);
            var userShoppingCard = user.UserCard;

            if (item.SelectedProductId != null && userShoppingCard != null)
            {
                var product = this.GetDetailsForProduct(item.SelectedProductId);
                if (product != null )
                {
                    BiletInShoppingCard itemToAdd = new BiletInShoppingCard
                    {
                        Id = Guid.NewGuid(),
                        Bilet = product,
                        BiletId = product.Id,
                        ShoppingCard = userShoppingCard,
                        ShoppingCartId = userShoppingCard.Id,
                        Quantity = item.Quantity

                    };
                    this._productInShoppingCartRepository.Insert(itemToAdd);
                    _logger.LogInformation("Product was successfully added into ShoppingCard.");
                    return true;
                }
                return false;
            }
            _logger.LogInformation("Something was wrong. BiletId or UserShoppingCard may be unavailable.");
            return false;   
        }

        public void CreateNewProduct(Billet p)
        {
            this._productRepository.Insert(p);

        }

        public void DeleteProduct(Guid id)
        {
            var product = this.GetDetailsForProduct(id);
            this._productRepository.Delete(product);    
        }

        public List<Billet> GetAllProducts()
        {
            _logger.LogInformation("GetAllProducts was callled.");
            return this._productRepository.GetAll().ToList();
        }

        public Billet GetDetailsForProduct(Guid? id)
        {
            return this._productRepository.Get(id);
        }

        public AddToShoppingCardDto GetShoppingCartInfo(Guid? id)
        {
            var billet = this.GetDetailsForProduct(id);
            AddToShoppingCardDto model = new AddToShoppingCardDto
            {
                SelectedProduct = billet,
                SelectedProductId = billet.Id,
                Quantity = 1
            };
            return model;
        }

        public void UpdeteExistingProduct(Billet p)
        {
            this._productRepository.Update(p);
        }
    }
}

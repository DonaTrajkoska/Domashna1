using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Repository.Interface;
using EShop.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShop.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCard> _shoppingCartRepository;
        private readonly IRepository<BiletInOrder> _productInOrderRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IUserRepository _userRepository;
       
        public ShoppingCartService(IRepository<ShoppingCard> shoppingCartRepository, IUserRepository userRepository, IRepository<Order> orderRepository, IRepository<BiletInOrder> productInOrderRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _productInOrderRepository = productInOrderRepository;
             
        }

        public bool deleteProductFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCard;

                var productToDelete = userShoppingCart.BiletInShoppingCards.
                    Where(z => z.BiletId == id).
                    FirstOrDefault();

                userShoppingCart.BiletInShoppingCards.Remove(productToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);
                return true;
            }
            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser =this._userRepository.Get(userId);
            var userShoppingCart = loggedInUser.UserCard;
            var allBilets = userShoppingCart.BiletInShoppingCards.Select(z => z.Bilet).ToList();
            var productPrice = userShoppingCart.BiletInShoppingCards.Select(z => new
            {
                ProductPrice = z.Bilet.BilletPrice,
                Quantity = z.Quantity
            }).ToList();
            double totalPrice = 0;
            foreach (var item in productPrice)
            {
                totalPrice += item.ProductPrice * item.Quantity;
            }
            ShoppingCartDto shoppingCardDtoitem = new ShoppingCartDto
            {
                TotalPrice = totalPrice,
                BiletInShoppingCards = userShoppingCart.BiletInShoppingCards.ToList(),
            };
            return shoppingCardDtoitem;     
        }

        public bool OrderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCard;
                
                

                Order item = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    User = loggedInUser
                };
                this._orderRepository.Insert(item); 

                List<BiletInOrder> biletInOrders = new List<BiletInOrder>();
                var result = userShoppingCart.BiletInShoppingCards
                    .Select(z => new BiletInOrder
                    {
                        Id = Guid.NewGuid(),  
                        OrderId = item.Id,
                        BiletId = z.Bilet.Id,
                        SelectedBilet = z.Bilet,
                        UserOrder = item,
                        Quantity=z.Quantity
                    }).ToList();

                
                biletInOrders.AddRange(result);
                foreach (var it in biletInOrders)
                {
                    this._productInOrderRepository.Insert(it);
                }
                loggedInUser.UserCard.BiletInShoppingCards.Clear();
                
                this._userRepository.Update(loggedInUser);
                return true;
            }
            return false;
            
        }
    }
}

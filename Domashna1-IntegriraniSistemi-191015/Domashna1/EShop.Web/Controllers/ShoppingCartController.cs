using EShop.Services.Interface;
using EShop.Web.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(this._shoppingCartService.getShoppingCartInfo(userId));
        }
        public  IActionResult DeleteProductFromShoppingCart(Guid productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result=this._shoppingCartService.deleteProductFromShoppingCart(userId,productId);
            if (result)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            else
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            
        }
        private Boolean OrderNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = this._shoppingCartService.OrderNow(userId);
            return result;
            
        }
        public IActionResult PayOrder(string stripeEmail,string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = this._shoppingCartService.getShoppingCartInfo(userId);
            
            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email=stripeEmail,
                Source=stripeToken
            });
            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount=(Convert.ToInt16(order.TotalPrice) * 100),
                Description="EShop Application Payment",
                Currency="usd",
                Customer=customer.Id
            });

            if (charge.Status == "succeeded")
            {
                var result = this.OrderNow();
                if (result)
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
                else
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
            }
            return null;

        }
    }
}

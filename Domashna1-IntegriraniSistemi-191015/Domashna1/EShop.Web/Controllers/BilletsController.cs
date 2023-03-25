using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using EShop.Web.Models.Identity;
using EShop.Repository;
using EShop.Services.Interface;
using EShop.Domain.DomainModels;
using EShop.Domain.DTO;

namespace EShop.Web.Controllers
{
    
    public class BilletsController : Controller
    {
        private readonly IProductService _productService;
   

        public BilletsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Billets
        public  IActionResult Index()
        {
            var allProducts = this._productService.GetAllProducts();
            return View(allProducts);
        }
        public IActionResult AddBilletToCard(Guid? id)
        {
            var model= this._productService.GetShoppingCartInfo(id);
            
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBilletToCard([Bind("SelectedProductId", "Quantity")] AddToShoppingCardDto item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = this._productService.AddToShoppingCart(item, userId);
            if (result)
            {
                return RedirectToAction("Index", "Billets");

            }
            return View(item);    

        }

        // GET: Billets/Details/5
        public  IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billet = this._productService.GetDetailsForProduct(id);
            if (billet == null)
            {
                return NotFound();
            }

            return View(billet);
        }

        // GET: Billets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Billets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,MovieName,MovieImage,MovieDescription,BilletPrice,MovieRating,Datum,Zanr")] Billet billet)
        {
            if (ModelState.IsValid)
            {
                this._productService.CreateNewProduct(billet);
                return RedirectToAction(nameof(Index));
            }
            return View(billet);
        }

        // GET: Billets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billet = this._productService.GetDetailsForProduct(id);
            if (billet == null)
            {
                return NotFound();
            }
            return View(billet);
        }

        // POST: Billets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(Guid id, [Bind("Id,MovieName,MovieImage,MovieDescription,BilletPrice,MovieRating,Datum,Zanr")] Billet billet)
        {
            if (id != billet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._productService.UpdeteExistingProduct(billet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BilletExists(billet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(billet);
        }

        // GET: Billets/Delete/5
        public  IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billet = this._productService.GetDetailsForProduct(id);

            if (billet == null)
            {
                return NotFound();
            }

            return View(billet);
        }

        // POST: Billets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        private bool BilletExists(Guid id)
        {
            return this._productService.GetDetailsForProduct(id) != null;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storage.Data;
using Storage.Models;

namespace Storage.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StorageContext _context;

        public ProductsController(StorageContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,OrderDate,Category,Shelf,Count,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,OrderDate,Category,Shelf,Count,Description")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        //GET: ProductViewModel/ListAllInventoryProducts
  
        public async Task<IActionResult> GetProductInventory()
        {
            var products = await _context.Product.ToListAsync();

            IEnumerable<ProductViewModel> productViewModelList = products.Select(product => new ProductViewModel
            {
                Name = product.Name,
                Price = product.Price,
                Count = product.ProductCount,
                InventoryValue = product.Price * product.ProductCount
            });
            return View(productViewModelList);
        }

        //GET: Product/ListAllProductsSearchByCategory

        public async Task<IActionResult> Search(string inputName,string inputCategory)
        {
            var products = await _context.Product.ToListAsync();

            if (inputCategory == "Select Category")
                inputCategory = string.Empty;

            IEnumerable<Product> filteredProducts = products;

            if (!string.IsNullOrEmpty(inputName) && !string.IsNullOrEmpty(inputCategory))
            {
                filteredProducts = products.Where(product => product.Name.Contains(inputName, StringComparison.OrdinalIgnoreCase) && product.Category.Contains(inputCategory, StringComparison.OrdinalIgnoreCase));
                                                        
            }
            else if (!string.IsNullOrEmpty(inputName) && string.IsNullOrEmpty(inputCategory))
            {
                filteredProducts = products.Where(product => product.Name.Contains(inputName, StringComparison.OrdinalIgnoreCase));
            }
            else if (string.IsNullOrEmpty(inputName) && !string.IsNullOrEmpty(inputCategory))
            {
                filteredProducts = products.Where(product => product.Category.Contains(inputCategory, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            var productFilteredList = filteredProducts.Select(product => new Product
            {
                Name = product.Name,
                Price = product.Price,
                ProductCount = product.ProductCount,
                OrderDate = product.OrderDate,
                Description = product.Description,
                Shelf = product.Shelf,
                Category = product.Category
            });
            return View("Index", productFilteredList);
        }


    }
}

﻿using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Models;

namespace PetsWebsite.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly PetsDBContext ProductDBContext;


        public ShoppingCartController(PetsDBContext _PetsDBContext)
        {
            ProductDBContext = _PetsDBContext;
        }

        //立即購買
        public async Task<IActionResult> Index(int id)
        {
            var products = ProductDBContext.Products
                .Where(m => m.ProductId == id)
                .ToList();

            return View(products);
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using PetsWebsite.Models;

namespace PetsWebsite.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PetsDBContext ProductDBContext;


        public CheckoutController(PetsDBContext _PetsDBContext)
        {
            ProductDBContext = _PetsDBContext;
        }

        IList<Product> GetProducts()
        {
            IList<Product> products = new List<Product>();
            products.Add(new Product { ProductId = 1, Discontinued = true, ProductName = "成貓罐頭150克【鮪魚+鮭魚】(1入)", UnitPrice = 500 });
            products.Add(new Product { ProductId = 2, Discontinued = true, ProductName = "食在好抓窩 時空羅盤軌道貓抓板", UnitPrice = 450 });
            products.Add(new Product { ProductId = 3, Discontinued = true, ProductName = "電動不倒翁逗貓棒 天藍(7x7x15公分)", UnitPrice = 500 });
            products.Add(new Product { ProductId = 4, Discontinued = true, ProductName = "電動不倒翁逗貓棒 天藍(7x7x15公分)", UnitPrice = 540 });
            products.Add(new Product { ProductId = 5, Discontinued = true, ProductName = "【買1送2組合 】汪老先生有塊蘿蔔園!", UnitPrice = 540 });
            products.Add(new Product { ProductId = 6, Discontinued = false, ProductName = "嬉皮河馬玩具(狗玩具)滿額現折", UnitPrice = 530 });
            products.Add(new Product { ProductId = 7, Discontinued = false, ProductName = "夏日寵物涼感巾 紅點點L號(38~83公分)", UnitPrice = 680 });
            products.Add(new Product { ProductId = 8, Discontinued = false, ProductName = "小鸚鵡貓薄荷逗貓棒 黃綠(9.2x4.6公分)", UnitPrice = 550 });
            products.Add(new Product { ProductId = 9, Discontinued = false, ProductName = "無線馬達寵物飲水機 1.7L ", UnitPrice = 450 });
            products.Add(new Product { ProductId = 10, Discontinued = false, ProductName = "無線馬達寵物飲水機 1.7L", UnitPrice = 350 });

            return products;
        }
        public async Task<IActionResult> Index(int id)
        {
            var products = GetProducts()
                .Where(m => m.ProductId == id)
                .ToList();

            //var products = GetProducts()
            //          .OrderBy(x => x.UnitPrice)
            //          .ToList();

            return View(products);
        }
    }
}

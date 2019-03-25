﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GearStore.Models;

namespace GearStore.Controllers
{
    public class ProductsController : Controller
    {
        private ElectronicComponentsSMEntities _dataContext = new ElectronicComponentsSMEntities();

        // GET: Products
        public async Task<ActionResult> Index(int? page, int? menu, int? category, int? manufacturer)
        {
            if (page < 1)
            {
                return RedirectToAction(nameof(Index), new { menu, category, manufacturer });
            }
            var products = _dataContext.Products.Include(p => p.Category).Include(p => p.Manufacturer);
            if (menu.HasValue)
            {
                products = products.Where(p => p.Category.MenuID == menu.Value);
            }
            if (category.HasValue)
            {
                products = products.Where(p => p.CategoryID == category.Value);
            }
            if (manufacturer.HasValue)
            {
                products = products.Where(p => p.ManufacturerID == manufacturer.Value);
            }
            var count = products.Count();
            var n = 8;
            var maxPage = count % n == 0 ? count / n : count / n + 1;
            page = page ?? 1;
            if (page > maxPage && count != 0)
            {
                return HttpNotFound();
            }
            ViewBag.MaxPage = maxPage;
            ViewBag.NowPage = page;
            ViewBag.Menu = menu;
            ViewBag.Category = category;
            ViewBag.Manufacturer = manufacturer;
            var skip = page.Value * n - n;
            return View(await products.OrderByDescending(p => p.UpdatedDate).Skip(skip).Take(n).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await _dataContext.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [HttpPost]
        public ActionResult AddToCart(int? id, int? quantity)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = _dataContext.Products.SingleOrDefault(p => p.ProductID == id && !p.Discontinued);
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            quantity = quantity ?? 1;
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new CartViewModel();
            }
            var cart = Session["Cart"] as CartViewModel;
            var item = cart.Items.Find(p => p.ProductID == id);
            if (item != null)
            {
                item.Quantity += quantity.Value;
            }
            else
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductID = obj.ProductID,
                    ProductName = obj.ProductName,
                    CategoryID = obj.CategoryID,
                    ManufacturerID = obj.ManufacturerID,
                    PhotoFilePatch = obj.PhotoFilePatch,
                    Price = obj.Price,
                    Quantity = quantity.Value,
                    Category = obj.Category,
                    Manufacturer = obj.Manufacturer
                });
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult RemoveFromCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cart = Session["Cart"] as CartViewModel;
            cart.Items.Remove(cart.Items.Find(p => p.ProductID == id));
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dataContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

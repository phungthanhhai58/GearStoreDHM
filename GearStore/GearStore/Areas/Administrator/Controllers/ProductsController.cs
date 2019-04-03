using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GearStore.Models;
using GearStore.Areas.Administrator.Models;
using GearStore.Areas.Administrator.Controllers.CustomAttributes;

namespace GearStore.Areas.Administrator.Controllers
{
    [AdminAuthorize]
    public class ProductsController : Controller
    {
        private ElectronicComponentsSMEntities db = new ElectronicComponentsSMEntities();

        // GET: Administrator/Products
        public async Task<ActionResult> Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Manufacturer);
            return View(await products.ToListAsync());
        }

        // GET: Administrator/Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Administrator/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "ManufacturerName");
            return View();
        }

        // POST: Administrator/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductViewModel product)
        {
            if (ModelState.IsValid && product.UploadImage != null)
            {
                var fileName = Path.GetFileName(product.UploadImage.FileName);
                var path = Path.Combine(Server.MapPath("~/images/products/"), fileName);
                product.UploadImage.SaveAs(path);
                db.Products.Add(new Product
                {
                    ProductName = product.ProductName.Trim(),
                    CategoryID = product.CategoryID,
                    ManufacturerID = product.ManufacturerID,
                    Price = product.Price,
                    PhotoFilePatch = fileName,
                    UnitsInStock = product.UnitsInStock,
                    UpdatedDate = product.UpdatedDate ?? DateTime.Now,
                    ReorderLevel = product.ReorderLevel,
                    Rating = product.Rating,
                    Discontinued = product.Discontinued,
                    Description = string.IsNullOrWhiteSpace(product.Description) ? null : product.Description.Trim(),
                    Details = string.IsNullOrWhiteSpace(product.Details) ? null : product.Details.Trim()
                });
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "ManufacturerName", product.ManufacturerID);
            return View(product);
        }
        // GET: Administrator/Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "ManufacturerName", product.ManufacturerID);
            return View(new ProductViewModel
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                CategoryID = product.CategoryID,
                ManufacturerID = product.ManufacturerID,
                Price = product.Price,
                PhotoFilePatch = product.PhotoFilePatch,
                UnitsInStock = product.UnitsInStock,
                UpdatedDate = product.UpdatedDate,
                ReorderLevel = product.ReorderLevel,
                Rating = product.Rating,
                Discontinued = product.Discontinued,
                Description = product.Description,
                Details = product.Details,
            });
        }

        // POST: Administrator/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (product.UploadImage != null)
                {
                    fileName = Path.GetFileName(product.UploadImage.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/products/"), fileName);
                    product.UploadImage.SaveAs(path);
                }
                db.Entry(new Product
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName.Trim(),
                    CategoryID = product.CategoryID,
                    ManufacturerID = product.ManufacturerID,
                    Price = product.Price,
                    PhotoFilePatch = fileName ?? product.PhotoFilePatch,
                    UnitsInStock = product.UnitsInStock,
                    UpdatedDate = product.UpdatedDate ?? DateTime.Now,
                    ReorderLevel = product.ReorderLevel,
                    Rating = product.Rating,
                    Discontinued = product.Discontinued,
                    Description = string.IsNullOrWhiteSpace(product.Description) ? null : product.Description.Trim(),
                    Details = string.IsNullOrWhiteSpace(product.Details) ? null : product.Details.Trim()
                }).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "ManufacturerName", product.ManufacturerID);
            return View(product);
        }

        // GET: Administrator/Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Administrator/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

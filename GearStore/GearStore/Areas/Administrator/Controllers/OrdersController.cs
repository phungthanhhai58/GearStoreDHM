using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GearStore.Models;
using GearStore.Areas.Administrator.Controllers.CustomAttributes;

namespace GearStore.Areas.Administrator.Controllers
{
    [AdminAuthorize(Order=3)]
    public class OrdersController : Controller
    {
        private ElectronicComponentsSMEntities _dataContext = new ElectronicComponentsSMEntities();

        // GET: Administrator/Orders
        public async Task<ActionResult> Index(int? page)
        {
            if (page < 1)
            {
                return RedirectToAction(nameof(Index));
            }
            var orders = _dataContext.Orders.Include(e => e.OrderDetails);
            var count = orders.Count();
            var n = 10;
            var maxPage = count % n == 0 ? count / n : count / n + 1;
            page = page ?? 1;
            if (page > maxPage && count != 0)
            {
                return HttpNotFound();
            }
            ViewBag.MaxPage = maxPage;
            ViewBag.NowPage = page;
            var skip = page.Value * n - n;
            return View(await orders.OrderByDescending(p => p.OrderDate).Skip(skip).Take(n).ToListAsync());
        }

        // GET: Administrator/Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await _dataContext.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        // GET: Administrator/Orders/Edit/5
        public async Task<ActionResult> Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await _dataContext.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(_dataContext.Customers, "CustomerID", "Username", order.CustomerID);
            return View(order);
        }

        // POST: Administrator/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update([Bind(Include = "OrderID,CustomerID,OrderDate,ShippedDate,Status,IsShipped,IsPaid")] Order order)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Entry(order).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(_dataContext.Customers, "CustomerID", "Username", order.CustomerID);
            return View(order);
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

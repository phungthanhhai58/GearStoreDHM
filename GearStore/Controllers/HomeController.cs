using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GearStore.Models;
using System.Data.Entity;

namespace GearStore.Controllers
{
    public class HomeController : Controller
    {
        private ElectronicComponentsSMEntities _dataContext = new ElectronicComponentsSMEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View(_dataContext.Products.OrderByDescending(p=>p.UpdatedDate).Take(6));
        }
        [ChildActionOnly]
        public ActionResult BestSeller()
        {
            return PartialView(_dataContext.Products.OrderByDescending(p => p.ReorderLevel).FirstOrDefault(p => !p.Discontinued) ?? new Product());
        }
        [ChildActionOnly]
        public ActionResult Menu()
        {
            return PartialView(_dataContext.Menus.Include(p => p.Categories));
        }
        [ChildActionOnly]
        public ActionResult MenuMobile()
        {
            return PartialView(_dataContext.Menus.Include(p => p.Categories));
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
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
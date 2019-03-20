using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GearStore.Models;

namespace GearStore.Controllers
{
    public class HomeController : Controller
    {
        private ElectronicComponentsSMEntities _dataContext = new ElectronicComponentsSMEntities();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.IsHomePage = true;
            return View(_dataContext.Products.OrderByDescending(p=>p.UpdatedDate).Take(6));
        }
        public ActionResult Menu()
        {
            return PartialView(_dataContext.Menus);
        }
        [ChildActionOnly]
        public ActionResult MenuItem(int id)
        {
            return PartialView(_dataContext.Categories.Where(p => p.MenuID == id));
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
    }
}
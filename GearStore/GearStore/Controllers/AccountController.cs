using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GearStore.Models;
namespace GearStore.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/SignIn
        public ActionResult SignIn()
        {
            return View();
        }
        // GET: Account/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost,ActionName("Signup")]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include ="")] Customer customer)
        {
            return View(nameof(SignIn));
        }
    }
}
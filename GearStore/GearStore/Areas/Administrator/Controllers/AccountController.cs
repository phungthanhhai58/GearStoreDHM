using GearStore.Areas.Administrator.Models;
using GearStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearStore.Areas.Administrator.Controllers
{
    public class AccountController : Controller
    {
        private ElectronicComponentsSMEntities _dataContext = new ElectronicComponentsSMEntities();
        // GET: Administrator/Account
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost, ActionName("SignIn")]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(EmployeeSignInViewModel account, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var isValid = _dataContext.Employees.SingleOrDefault(p => p.Username == account.Username);
                if (isValid == null)
                {
                    ViewBag.Message = "Tài khoản không tồn tại.";
                    return View(account);
                }
                else if (isValid.Password != account.Password)
                {
                    ViewBag.Message = "Mật khẩu bị sai.";
                    return View(account);
                }
                HttpCookie userCookie = new HttpCookie("AdminAccount", account.Username);
                userCookie["ID"] = isValid.EmployeeID.ToString();
                userCookie["Username"] = account.Username;
                userCookie["Password"] = account.Password;
                userCookie["JobID"] = isValid.JobTitleID.ToString();
                userCookie.Expires = DateTime.Now.AddDays(1);
                Response.SetCookie(userCookie);
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(account);
        }
        public ActionResult SignOut()
        {
            HttpCookie userCookie = new HttpCookie("AdminAccount");
            userCookie.Expires = DateTime.Now.AddDays(-1);
            Response.SetCookie(userCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}
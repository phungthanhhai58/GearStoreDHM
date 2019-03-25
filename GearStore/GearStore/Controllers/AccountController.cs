using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GearStore.Models;
namespace GearStore.Controllers
{
    public class AccountController : Controller
    {
        private ElectronicComponentsSMEntities _dataContext = new ElectronicComponentsSMEntities();
        // GET: Account/SignIn
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost, ActionName("SignIn")]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn([Bind(Include = "Username, Password")] SignInViewModel account)
        {
            if (ModelState.IsValid)
            {
                var isValid = _dataContext.Customers.SingleOrDefault(p => p.Username == account.Username);
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
                HttpCookie userCookie = new HttpCookie("Account", account.Username);
                userCookie["ID"] = isValid.CustomerID.ToString();
                userCookie["Username"] = account.Username;
                userCookie["Password"] = account.Password;
                userCookie.Expires = DateTime.Now.AddDays(1);
                Response.SetCookie(userCookie);
                return RedirectToAction("Index", "Home");

            }
            return View(account);
        }
        // GET: Account/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost, ActionName("SignUp")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp([Bind(Include = "Username, Password, ConfirmPassword, Email, FullName ,BirthDate, Gender, Address, PhoneNumber")] SignUpViewModel account)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Username = account.Username,
                    Password = account.Password,
                    Email = account.Email,
                    FullName = account.FullName,
                    Gender = account.Gender,
                    BirthDate = account.BirthDate,
                    PhoneNumber = account.PhoneNumber,
                    Address = account.Address
                };
                _dataContext.Customers.Add(customer);
                await _dataContext.SaveChangesAsync();
                return View(nameof(SignIn));
            }
            return View(account);
        }

        public ActionResult SignOut()
        {
            HttpCookie userCookie = new HttpCookie("Account");
            userCookie.Expires = DateTime.Now.AddDays(-1);
            Response.SetCookie(userCookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Details()
        {
            return Content(Request.Cookies["Account"]?["UserName"] ?? "404 NotFound Account");
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
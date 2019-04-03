using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GearStore.Models;

namespace GearStore.Controllers.CustomAttributes
{
    public class UserAuthorize : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool isFailed = false;
            string message = "";
            var account = filterContext.HttpContext.Request.Cookies["Account"];
            if (account == null)
            {
                message = "Đăng nhập để tiếp tục.";
                isFailed = true;
            }
            else
            {
                using (var dataContext = new ElectronicComponentsSMEntities())
                {
                    var id = int.Parse(account["ID"]);
                    var model = dataContext.Customers.Find(id);
                    if (model.IsDisabled)
                    {
                        message = "Tài khoản đã bị khóa";
                        isFailed = true;
                    }
                    else if (model.Password != account["Password"])
                    {
                        message = "Mật khẩu đã bị thay đổi, vui lòng đăng nhập lại";
                        isFailed = true;
                    }
                }

            }
            if (isFailed)
            {
                HttpCookie userCookie = new HttpCookie("Account");
                userCookie.Expires = DateTime.Now.AddDays(-1);
                filterContext.HttpContext.Response.SetCookie(userCookie);
                filterContext.Controller.TempData["Message"] = message;
                filterContext.Result = new RedirectResult("/Account/SignIn?returnUrl=" + filterContext.HttpContext.Request.Url.PathAndQuery);
            }
        }
    }
}
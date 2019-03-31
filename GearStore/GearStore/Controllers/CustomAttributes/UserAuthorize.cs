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
            var account = filterContext.HttpContext.Request.Cookies["Account"];
            if (account == null)
            {
                filterContext.Controller.TempData["Message"] = "Đăng nhập để tiếp tục.";
                filterContext.Result = new RedirectResult("/Account/SignIn");
            }
        }
    }
}
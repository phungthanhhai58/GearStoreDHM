using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GearStore.Models;

namespace GearStore.Areas.Administrator.Controllers.CustomAttributes
{
    public class AdminAuthorize : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var account = filterContext.HttpContext.Request.Cookies["AdminAccount"];
            if (account == null)
            {
                filterContext.Controller.TempData["Message"] = "Đăng nhập để tiếp tục.";
                filterContext.Result = new RedirectResult("/Account/SignIn");
            }
        }
    }
}
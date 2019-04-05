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
            bool isFailed = false;
            string message = "";
            var account = filterContext.HttpContext.Request.Cookies["AdminAccount"];
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
                    var model = dataContext.Employees.Find(id);
                    if (model.JobTitleID == null)
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
                HttpCookie userCookie = new HttpCookie("AdminAccount");
                userCookie.Expires = DateTime.Now.AddDays(-1);
                filterContext.HttpContext.Response.SetCookie(userCookie);
                filterContext.Controller.TempData["Message"] = message;
                filterContext.Result = new RedirectResult("/Administrator/Account/SignIn");
            }
            else if (Order!=-1 && !Check(Order,int.Parse(account["JobID"] ?? "0")))
            {
                filterContext.Controller.TempData["Message"] = "Bạn không có quyền hạn truy cập.";
                filterContext.Result = new RedirectResult("/Administrator/Account/SignIn");
            }
        }
        private static bool Check(int Order, int JobID)
        {
            switch (Order)
            {
                //Toàn quyền
                //CRUD toàn bộ
                case 1:
                    switch (JobID)
                    {
                        case 1:
                            return true;
                        default:
                            return false;
                    }
                // Nhập kho
                // CRUD Hàng hóa, Loại, Hãng, Menu
                case 2:
                    switch (JobID)
                    {
                        case 1:
                        case 2:
                            return true;
                        default:
                            return false;
                    }
                // Kiểm đơn
                // Đánh dấu tình trạng đơn hàng
                case 3:
                    switch (JobID)
                    {
                        case 1:
                        case 3:
                            return true;
                        default:
                            return false;
                    }
                // Quản lý
                // Xem báo cáo thống kê
                case 4:
                    switch (JobID)
                    {
                        case 1:
                        case 4:
                            return true;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }
    }
}
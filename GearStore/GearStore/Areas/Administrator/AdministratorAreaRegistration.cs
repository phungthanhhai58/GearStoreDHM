using System.Web.Mvc;

namespace GearStore.Areas.Administrator
{
    public class AdministratorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administrator";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Administrator_default",
                "Administrator/{controller}/{action}/{id}",
                new { action = "Index",controller = "Home", id = UrlParameter.Optional },
                namespaces: new[] { "GearStore.Areas.Administrator.Controllers" }
            );
        }
    }
}
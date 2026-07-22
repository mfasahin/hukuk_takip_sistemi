using System.Web.Mvc;

namespace Presentation.Filters
{
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;

            // Login/Register/Logout action'larını hariç tut - sonsuz döngüye girmemek için
            bool isAuthAction = controller == "Kullanici" &&
                (action == "Login" || action == "Register" || action == "Logout");

            if (!isAuthAction && filterContext.HttpContext.Session["KullaniciId"] == null)
            {
                filterContext.Result = new RedirectResult("~/Kullanici/Login");
            }
        }
    }
}
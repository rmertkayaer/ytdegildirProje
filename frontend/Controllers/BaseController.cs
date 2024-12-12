using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace frontend.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Skip authentication check for Account controller actions
            if (context.Controller is AccountController)
                return;

            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Make user information available to all views
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            ViewData["UserEmail"] = HttpContext.Session.GetString("UserEmail");
        }
    }
} 
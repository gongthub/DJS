using DJS.Common;
using System.Web.Mvc;

namespace DJS.WebApp
{
    public class HandlerLoginAttribute : AuthorizeAttribute
    {
        public bool Ignore = true;
        public HandlerLoginAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore == false)
            {
                return;
            }
            if (OperatorProvider.Provider.GetCurrent() == null)
            {
                WebHelper.WriteCookie("djs_login_error", "overdue");
                filterContext.HttpContext.Response.Redirect("/Login/Index");
                return;
            }
        }
    }
}
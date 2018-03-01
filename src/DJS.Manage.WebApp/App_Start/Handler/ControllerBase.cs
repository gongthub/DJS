using Microsoft.AspNetCore.Mvc;

namespace DJS.Manage.WebApp
{
    public abstract class ControllerBase : Controller
    {
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Form()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Details()
        {
            return View();
        }
        //protected virtual ActionResult Success(string message)
        //{
        //    return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        //}
        //protected virtual ActionResult Success(string message, object data)
        //{
        //    return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        //}
        //protected virtual ActionResult Error(string message)
        //{
        //    return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
        //}
    }
}

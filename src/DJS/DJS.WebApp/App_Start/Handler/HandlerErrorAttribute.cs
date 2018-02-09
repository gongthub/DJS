using DJS.WebApp.Models;
using DJS.Common;
using System.Web.Mvc;

namespace DJS.WebApp
{
    public class HandlerErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;
            context.Result = new ContentResult { Content = new AjaxResult { state = ResultType.error.ToString(), message = context.Exception.Message }.ToJson() };
            WriteLog(context);
        }
        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
                return;
            LogHelp.logHelp.WriteLog(context.Exception.Message, Enums.LogType.Error);

        }
    }
}
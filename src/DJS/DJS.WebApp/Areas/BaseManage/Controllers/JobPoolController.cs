using DJS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DJS.WebApp.Areas.BaseManage.Controllers
{
    public class JobPoolController : ControllerBase
    {
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = BLL.Jobs.GetJobPoolList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DoWork(string keyValue)
        {
            try
            {
                bool bState = BLL.Jobs.TriggerJob(keyValue);
                if (bState)
                {
                    return Success("操作成功。");
                }
                else
                {
                    return Error("操作失败。");
                }
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Stop(string keyValue)
        {
            try
            {
                bool bState = BLL.Jobs.PauseJob(keyValue);
                if (bState)
                {
                    return Success("操作成功。");
                }
                else
                {
                    return Error("操作失败。");
                }
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Run(string keyValue)
        {
            try
            {
                bool bState = BLL.Jobs.ResumeJob(keyValue);
                if (bState)
                {
                    return Success("操作成功。");
                }
                else
                {
                    return Error("操作失败。");
                }
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(string keyValue)
        {
            try
            {
                bool bState = BLL.Jobs.DelByIdForQuartz(keyValue);
                if (bState)
                {
                    return Success("操作成功。");
                }
                else
                {
                    return Error("操作失败。");
                }
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
    }
}

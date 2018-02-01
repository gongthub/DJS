using DJS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DJS.WebApp.Areas.BaseManage.Controllers
{
    public class JobMgrController : ControllerBase
    {
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = BLL.Jobs.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = BLL.Jobs.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTypeGridJson()
        {
            List<SelectLists> list = Common.EnumHelp.enumHelp.ToSelectLists(typeof(Enums.TimeType));
            return Content(list.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Model.Jobs modelEntity, string keyValue)
        {
            try
            {
                bool bState = BLL.Jobs.SubmitForm(modelEntity, keyValue);
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
        public ActionResult DeleteForm(string keyValue)
        {
            try
            {
                bool bState = BLL.Jobs.DeleteByID(keyValue);
                if (bState)
                {
                    return Success("删除成功。");
                }
                else
                {
                    return Error("删除失败。");
                }
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
    }
}

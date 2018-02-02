using DJS.Common;
using DJS.Common.CommonModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DJS.WebApp.Areas.BaseManage.Controllers
{
    public class DllMgrController : ControllerBase
    {
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = BLL.DllMgr.GetList(pagination, keyword),
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
            var data = BLL.DllMgr.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectGridJson()
        {
            var data = BLL.DllMgr.GetList();
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetDllNameGridJson(string keyValue)
        {
            List<SelectLists> list = BLL.DllMgr.GetDllNameList(keyValue);
            return Content(list.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Model.DllMgr modelEntity, string keyValue)
        {
            try
            {
                bool bState = BLL.DllMgr.SubmitForm(modelEntity, keyValue);
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
                bool bState = BLL.DllMgr.DelById(keyValue);
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

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult UpForm()
        {
            return View();
        }
    }
}

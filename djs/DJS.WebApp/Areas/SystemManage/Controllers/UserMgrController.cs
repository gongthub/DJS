using DJS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DJS.BLL;
using DJS.Model;

namespace DJS.WebApp.Areas.SystemManage.Controllers
{
    public class UserMgrController : ControllerBase
    {
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = BLL.UserMgr.GetList(pagination, keyword),
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
            var data = BLL.UserMgr.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(UserEntity userEntity, string keyValue)
        {
            try
            {
                bool bState = BLL.UserMgr.SubmitForm(userEntity, keyValue);
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
                bool bState = BLL.UserMgr.RemoveByID(keyValue);
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
        public ActionResult RevisePassword()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRevisePassword(string userPassword, string keyValue)
        {
            bool bState = BLL.UserMgr.RevisePassword(keyValue, userPassword);
            if (bState)
            {
                return Success("重置密码成功。");
            }
            else
            {
                return Error("重置密码失败。");
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledAccount(string keyValue)
        {
            bool bState = BLL.UserMgr.DisabledAccount(keyValue);
            if (bState)
            {
                return Success("账户禁用成功。");
            }
            else
            {
                return Error("账户禁用失败。");
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledAccount(string keyValue)
        {
            bool bState = BLL.UserMgr.EnabledAccount(keyValue);
            if (bState)
            {
                return Success("账户启用成功。");
            }
            else
            {
                return Error("账户启用失败。");
            }
        }

        [HttpGet]
        public ActionResult Info()
        {
            return View();
        }
    }
}

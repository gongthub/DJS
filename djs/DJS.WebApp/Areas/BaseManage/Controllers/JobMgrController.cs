using DJS.Common;
using System;
using System.Collections;
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
                bool bState = BLL.Jobs.RemoveByID(keyValue);
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
        public ActionResult Configs()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetJobConfigs(string keyValue)
        {
            var data = BLL.Jobs.GetConfigs(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetConfigTypeGridJson()
        {
            List<SelectLists> list = Common.EnumHelp.enumHelp.ToSelectLists(typeof(Enums.ConfigGetType));
            return Content(list.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitConfigs(string keyValue)
        {
            try
            {
                bool bState = false;
                string hdConfigSels = Request["hdConfigSels"];
                if (hdConfigSels != null)
                {
                    List<string> configNames = hdConfigSels.Split('&').ToList();
                    configNames = configNames.Where(m => !string.IsNullOrEmpty(m)).ToList();
                    if (configNames != null && configNames.Count > 0)
                    {
                        List<SelectStrLists> configSels = new List<SelectStrLists>();
                        foreach (var configName in configNames)
                        {
                            string configVal = Request[configName];
                            if (configVal != null)
                            {
                                SelectStrLists configSel = new SelectStrLists();
                                configSel.Name = configName;
                                configSel.Value = configVal;
                                configSels.Add(configSel);
                            }
                        }
                        bState = BLL.Jobs.SaveConfigs(keyValue, configSels);
                    }
                }

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

        [HttpGet]
        [HandlerAuthorize]
        public ActionResult Files()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetJobFiles(string keyValue)
        {
            var data = BLL.Jobs.GetJobFiles(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitJobFiles()
        {
            try
            {
                if (Request["jobFiles"] != null)
                {
                    List<Model.JobFiles> jobFiles = JsonHelp.ToObject<List<Model.JobFiles>>(Request["jobFiles"].ToString());
                    BLL.Jobs.SaveJobFiles(jobFiles);

                }
                return Success("操作成功。");
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
        public ActionResult DeleteFile(string keyValue)
        {
            try
            {
                BLL.Jobs.DeleteFile(keyValue);
                return Success("删除成功。");
            }
            catch (Exception e)
            {
                return Error("删除失败。");
            }
        }


        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult AddPools(string keyValue)
        {
            try
            {
                bool bState = BLL.Jobs.AddJobs(keyValue);
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

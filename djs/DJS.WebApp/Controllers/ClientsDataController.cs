using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DJS.Common;
using DJS.Model;
using System;

namespace DJS.WebApp.Controllers
{
    [HandlerLogin]
    public class ClientsDataController : Controller
    {
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetClientsDataJson()
        {
            var data = new
            {
                authorizeMenu = this.GetMenuList()
            };
            return Content(data.ToJson());
        }
        private object GetMenuList()
        {
            List<ModuleEntity> models = DJS.BLL.ModuleMgr.GetModels();
            return ToMenuJson(models, "0");
        }
        private string ToMenuJson(List<ModuleEntity> data, string parentId)
        {
            StringBuilder sbJson = new StringBuilder();
            sbJson.Append("[");
            List<ModuleEntity> entitys = data.FindAll(t => t.ParentId == parentId);
            if (entitys.Count > 0)
            {
                foreach (var item in entitys)
                {
                    string strJson = item.ToJson();
                    strJson = strJson.Insert(strJson.Length - 1, ",\"ChildNodes\":" + ToMenuJson(data, item.ID) + "");
                    sbJson.Append(strJson + ",");
                }
                sbJson = sbJson.Remove(sbJson.Length - 1, 1);
            }
            sbJson.Append("]");
            return sbJson.ToString();
        }
    }
}

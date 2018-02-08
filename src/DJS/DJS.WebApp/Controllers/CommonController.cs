using DJS.Common.CommonModel;
using DJS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DJS.WebApp.Controllers
{
    public class CommonController : ControllerBase
    {
        [HttpPost]
        [HandlerAuthorize]
        public ActionResult UploadFile()
        {
            try
            {
                string uploadType = string.Empty;
                string dirName = string.Empty;

                if (HttpContext.Request["uploadtype"] != null)
                {
                    uploadType = Request["uploadtype"];
                }
                if (HttpContext.Request["dirname"] != null)
                {
                    dirName = Request["dirname"];
                }
                UpFileDTO entity = new UpFileDTO();
                entity.IsNoFile = false;
                if (HttpContext.Request.Files.Count > 0)
                {
                    var upFiles = HttpContext.Request.Files[0];
                    if (upFiles != null)
                    {
                        entity = Common.HttpUploadFileHelp.httpUploadFileHelp.UpLoadFileByType(upFiles, uploadType, dirName);
                    }
                }
                else
                {
                    entity.IsNoFile = true;
                    return Success("true", entity);
                }
                return Success("true", entity);

            }
            catch (Exception ex)
            {
                return Success("false", ex.Message);
            }
        }


        [HttpPost]
        [HandlerAuthorize]
        public ActionResult UploadFiles()
        {
            try
            {
                string uploadType = string.Empty;
                string dirName = string.Empty;

                if (HttpContext.Request["uploadtype"] != null)
                {
                    uploadType = Request["uploadtype"];
                }
                if (HttpContext.Request["dirname"] != null)
                {
                    dirName = Request["dirname"];
                }
                List<UpFileDTO> entitys = new List<UpFileDTO>();
                if (HttpContext.Request.Files.Count > 0)
                {
                    var upFiles = HttpContext.Request.Files;
                    for (int i = 0; i < upFiles.Count; i++)
                    {
                        UpFileDTO entity = new UpFileDTO();
                        entity = Common.HttpUploadFileHelp.httpUploadFileHelp.UpLoadFileByType(upFiles[i], uploadType, dirName);
                        entitys.Add(entity);
                    }
                }
                else
                {
                    return Success("true");
                }
                return Success("true", entitys);

            }
            catch (Exception ex)
            {
                return Success("false", ex.Message);
            }
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetJobNums()
        {
            List<Model.Jobs> runningJobs = BLL.Jobs.GetRunningJobs();
            int runningJobNums = BLL.Jobs.GetRunningJobNums();
            int jobNums = BLL.Jobs.GetJobNums();
            int jobPoolNums = BLL.Jobs.GetJobsForQuartzCount();
            var data = new { runningJobs = runningJobs, runningJobNums = runningJobNums, jobNums = jobNums, jobPoolNums = jobPoolNums };
            return Content(data.ToJson());
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetLogs()
        {
            bool isChange = Common.LogHelp.logHelp.IsChangeLog();
            string logs = string.Empty;
            if (isChange)
            {
                logs = Common.LogHelp.logHelp.GetLogs(100);
            }
            var data = new { isChange = isChange, logs = logs };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetLogFiles()
        {
            List<LogFileModel> logFiles = Common.LogHelp.logHelp.GetLogFiles();
            var data = new { logFiles = logFiles };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCpuMemorys()
        {
            List<CpuMemoryModel> models = Common.CpuMemoryHelp.GetCpuMemorys();
            List<string> lbls = models.Select(m => m.Times.ToString("HH:mm:ss")).ToList();
            List<string> cpus = models.Select(m => m.cpu.ToString()).ToList();
            List<string> memorys = models.Select(m => m.memory.ToString()).ToList();
            var data = new { lbls = lbls, cpus = cpus, memorys = memorys };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCpuMemory()
        {
            CpuMemoryModel model = Common.CpuMemoryHelp.GetCpuMemory();
            var data = new { lbl = model.Times.ToString("mm:ss"), cpus = model.cpu, memorys = model.memory };
            return Content(data.ToJson());
        }

    }
}

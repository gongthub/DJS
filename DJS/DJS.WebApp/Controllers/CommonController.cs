using DJS.Common.CommonModel;
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
    }
}

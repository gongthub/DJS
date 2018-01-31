using DJS.Common.CommonModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DJS.Common
{
    public class HttpUploadFileHelp
    {
        #region 单例模式创建对象
        //单例模式创建对象
        private static HttpUploadFileHelp _httpUploadFileHelp = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        HttpUploadFileHelp()
        {
        }

        public static HttpUploadFileHelp httpUploadFileHelp
        {
            get
            {
                // Double-Checked Locking
                if (null == _httpUploadFileHelp)
                {
                    lock (SynObject)
                    {
                        if (null == _httpUploadFileHelp)
                        {
                            _httpUploadFileHelp = new HttpUploadFileHelp();
                        }
                    }
                }
                return _httpUploadFileHelp;
            }
        }
        #endregion
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public UpFileDTO UpLoadFileByType(HttpPostedFileBase file, string uploadType,string dirName="")
        {
            UpFileDTO entity = new UpFileDTO();

            switch (uploadType)
            { 
                case "dll":
                    string filePaths=GenFilePath(uploadType,dirName);
                    entity = UpLoadDllFile(file,filePaths);
                    break;
            }
            return entity;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public UpFileDTO UpLoadDllFile(HttpPostedFileBase file, string filePath)
        {
            //验证 
            VerifyFile(file, ConfigHelp.UPLOADDLLEXT);
            return UpLoadFile(file, filePath);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public UpFileDTO UpLoadFile(HttpPostedFileBase file, string filePath)
        {
            UpFileDTO entity = new UpFileDTO();
            string filePaths = string.Empty;
            string upPaths = filePath;
            string upPathsT = upPaths.Replace("~", "");
            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);// 原始文件名称
                string fileExtension = Path.GetExtension(fileName); // 文件扩展名  

                // 文件上传后的保存路径 
                filePath = InitSavePath(upPaths);
                string saveName = file.FileName; // 保存文件名称
                filePaths = upPathsT + saveName;
                file.SaveAs(filePath + saveName);

                entity.Sys_FileName = saveName;
                entity.Sys_FileOldName = fileName;
                entity.Sys_ExtName = fileExtension;
                entity.Sys_FilePath = filePaths;
            }
            return entity;
        }


        #region 上传文件验证
        /// <summary>
        /// 上传文件验证
        /// </summary>
        private void VerifyFile(HttpPostedFileBase file, string sysExt)
        {
            string fileName = Path.GetFileName(file.FileName);// 原始文件名称
            string fileExtension = Path.GetExtension(fileName); // 文件扩展名  

            //验证格式
            string formatFiles = string.Empty;
            if (!VerifyFileFormat(fileExtension, sysExt, out formatFiles))
            {
                throw new Exception("上传文件格式不符合要求，请重新上传！允许上传格式：" + formatFiles);
            }

        }

        /// <summary>
        /// 验证文件格式是否满足条件
        /// </summary>
        /// <param name="formats"></param>
        /// <returns></returns>
        private bool VerifyFileFormat(string formats, string sysExt)
        {
            bool bState = false;
            string formatImgs = sysExt;
            if (!string.IsNullOrEmpty(formats))
            {
                if (formatImgs != "*")
                {
                    string[] imgs = formatImgs.Split('|');
                    bState = imgs.Contains(formats);
                }
                else
                {
                    bState = true;
                }
            }
            return bState;
        }

        /// <summary>
        /// 验证文件格式是否满足条件
        /// </summary>
        /// <param name="formats"></param>
        /// <returns></returns>
        private bool VerifyFileFormat(string formats, string sysExt, out string formatStr)
        {
            bool bState = false;
            string formatImgs = sysExt;
            if (!string.IsNullOrEmpty(formats))
            {
                if (formatImgs != "*")
                {
                    string[] imgs = formatImgs.Split('|');
                    bState = imgs.Contains(formats);
                }
                else
                {
                    bState = true;
                }
            }
            formatStr = formatImgs;
            return bState;
        }

        #endregion

        /// <summary>
        /// 根据路径初始化保存文件夹
        /// </summary>
        /// <param name="filePaths"></param>
        private string InitSavePath(string filePaths)
        {
            // 文件上传后的保存路径 
            string filePath = Common.FileHelp.GetFullPath(filePaths);
            if (!Common.FileHelp.DirectoryIsExists(filePath))
            {
                Common.FileHelp.CreateDirectory(filePath);

            }

            return filePath;
        }

        /// <summary>
        /// 生成文件保存目录
        /// </summary>
        /// <returns></returns>
        private static string GenFilePath(string uploadType,string name)
        {
            string strPaths = string.Empty;
            switch (uploadType)
            {
                case "dll":
                    strPaths= ConfigHelp.AssemblySrcPath + name + "/";
                    break;
            }
            return strPaths;
        }

    }
}

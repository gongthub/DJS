using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanSummaryNewService.Utils
{
    public class FileOper
    {
        private string errMessage; //保存的错误信息
        public string ErrMessage
        {
            set
            {
                this.errMessage = value;
            }
            get
            {
                return this.errMessage;
            }
        }
        public FileOper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            errMessage = string.Empty;
        }


        /// <summary>
        /// 写文件操作,返回bool形数据
        /// </summary>
        /// <param name="fileDir">相对于根目录的文件路径，如update\</param>
        /// <param name="fileName">文件名，如temp.doc</param>
        /// <param name="txtStream">文本流，要输入的文本流信息</param>
        public bool writeOneFile(string fileDir, string fileName, string txtStream)
        {
            bool returnValue = true;
            string strDirPath = fileDir;//文件夹路径
            //如果文件夹不存在,则创建一个
            if (!Directory.Exists(strDirPath))
            {
                Directory.CreateDirectory(strDirPath);
            }
            StreamWriter sw = null;
            string fullPath = strDirPath + fileName;//完整目录
            Encoding encod = Encoding.GetEncoding("gb2312");//设置编码
            try  //开始写文件
            {
                sw = new StreamWriter(fullPath, false, encod);
                sw.Write(txtStream);
                sw.Flush();
            }
            catch (Exception exp)
            {
                this.errMessage = exp.ToString();
                returnValue = false;
            }
            finally
            {
                sw.Close();
            }
            return returnValue;
        }
        /// <summary>
        /// 读文件操作,返回string数据
        /// </summary>
        /// <param name="fileDir">根目录下的文件路径，如update\</param>
        /// <param name="fileName">文件名，如temp.doc</param>
        public string readOneFile(string fileDir, string fileName)
        {
            string returnValue = "没有读取到数据";
            string fullPath = fileDir + fileName;//完整目录
            Encoding encod = Encoding.GetEncoding("gb2312");//设置编码
            StreamReader sr = null;
            try  //读文件
            {
                if (ifHaveThisFile(fileDir + fileName))
                {
                    sr = new StreamReader(fullPath, encod);
                    returnValue = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception exp)
            {
                this.errMessage = exp.ToString();
                returnValue = returnValue + exp.ToString();
            }
            return returnValue;
        }
        /// <summary>
        /// 复制文件操作,返回bool形数据
        /// </summary>
        /// <param name="sourceFileFullPath">源文件完整路径,如：\upload\a.rar</param>
        /// <param name="newFileFullPath">目的文件完整路径,如：\upload\b.rar</param>
        public bool copyOneFile(string sourceFileFullPath, string newFileFullPath)
        {
            bool returnValue = false;

            try
            {
                if (ifHaveThisFile(sourceFileFullPath))
                {
                    File.Copy(sourceFileFullPath, newFileFullPath, true);
                    returnValue = true;
                }
            }
            catch (Exception exp)
            {
                this.errMessage = exp.ToString();
            }
            return returnValue;
        }

        /// <summary>
        /// 删除文件操作,返回bool形数据
        /// </summary>
        /// <param name="filePath">所要删除的文件的相对路径（相对根目录）</param>
        public bool deleteOneFile(string filePath)
        {
            bool returnValue = true;
            try
            {
                File.Delete(filePath);
            }
            catch (Exception exp)
            {
                this.errMessage = exp.ToString();
                returnValue = false;
            }
            return returnValue;
        }
        /// <summary>
        /// 判断文件是否存在,返回bool形数据
        /// </summary>
        /// <param name="fileFullPath">文件完整路径</param>
        public bool ifHaveThisFile(string fileFullPath)
        {
            bool returnValue = false;
            try
            {
                if (File.Exists(fileFullPath))
                    returnValue = true;
            }
            catch (Exception exp)
            {
                this.errMessage = exp.ToString();
            }
            return returnValue;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="FilePath"></param>
        public void CreateDirectory(string FilePath)
        {

            if (!Directory.Exists(FilePath)) //创建文件夹
            {
                Directory.CreateDirectory(FilePath);
            }

        }

        public void ExistFile(string strFileName)
        {

            if (System.IO.File.Exists(strFileName))
            {
                System.IO.File.Delete(strFileName);
            }
        }

    }
}

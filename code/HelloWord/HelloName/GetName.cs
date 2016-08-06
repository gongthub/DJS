using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloName
{
    public class GetName : IJob
    {

        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>
        private static DJS.SDK.ILog iLog = null;

        #endregion

        #region 构造函数

        static GetName()
        {
            try
            {
                iLog = DJS.SDK.DataAccess.CreateILog();
            }
            catch (Exception ex)
            {
                iLog.WriteLog( ex.Message, 1); 
            }
        }

        #endregion

        /// <summary>
        /// 实现接口
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {


                System.Threading.Thread.Sleep(2000);

                string namespaces = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;
                //数据会写
                iLog.WirteDatas(namespaces, "HelloName!" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //日志
                iLog.WriteLog("HelloName执行成功！", 0);
            }
            catch (Exception ex)
            {
                //日志
                iLog.WriteLog("HelloName执行失败！" + ex.Message, 1); 
            }
        }

        public void write(string str)
        {

            FileInfo finfo = new FileInfo(@"D:\test.txt");
            using (FileStream fs = finfo.OpenWrite())
            {
                //根据上面创建的文件流创建写数据流 
                StreamWriter strwriter = new StreamWriter(fs);
                //设置写数据流的起始位置为文件流的末尾 
                strwriter.BaseStream.Seek(0, SeekOrigin.End);
                //写入相关记录信息
                strwriter.WriteLine(str);
                //清空缓冲区内容，并把缓冲区内容写入基础流 
                strwriter.Flush();
                strwriter.Close();
                fs.Close();
            }
        }
    }
}

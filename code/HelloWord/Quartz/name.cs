using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz
{
    public class Name : IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            string fileLogPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileLogName = "TestQuartz_" + DateTime.Now.ToLongDateString() + "_log.txt";
            FileInfo finfo = new FileInfo(fileLogPath + fileLogName);
            using (FileStream fs = finfo.OpenWrite())
            {
                //根据上面创建的文件流创建写数据流 
                StreamWriter strwriter = new StreamWriter(fs);
                //设置写数据流的起始位置为文件流的末尾 
                strwriter.BaseStream.Seek(0, SeekOrigin.End);
                //写入相关记录信息
                strwriter.WriteLine("发生时间: " + DateTime.Now.ToString());
                strwriter.WriteLine("---------------------------------------------");
                strwriter.WriteLine();
                //清空缓冲区内容，并把缓冲区内容写入基础流 
                strwriter.Flush();
                strwriter.Close();
                fs.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DJS.Core.Common.File
{
    public class FileHelp
    {
        public static byte[] GetFileStram(string filePaths)
        {
            //filePaths = Path.GetFullPath(filePaths);
            using (FileStream fs = new FileStream(filePaths, FileMode.Open))
            {
                //获取文件大小
                long size = fs.Length;

                byte[] array = new byte[size];

                //将文件读到byte数组中
                fs.Read(array, 0, array.Length);

                fs.Close();
                return array;
            }
        }
        //private static Assembly GetAsm(string assemblyFile)
        //{
        //    Assembly asm;
        //    byte[] fileData = File.ReadAllBytes(assemblyFile);
        //    asm = Assembly.Load(fileData);
        //    return asm;
        //}
    }
}

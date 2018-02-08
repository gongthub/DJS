using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Assemblies;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common
{
    public class AssemblyHelp
    {
        /// <summary>
        /// 获取程序集文件所在文件夹名称
        /// </summary>
        private static string PATH = ConfigHelp.AssemblySrcPath;

        /// <summary>
        /// 程序集绝对路径
        /// </summary>
        private static string FULLPATH = FileHelp.GetFullPath(PATH);


        #region 单例模式创建对象
        //单例模式创建对象
        private static AssemblyHelp _assembly = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        AssemblyHelp()
        {
        }

        public static AssemblyHelp assembly
        {
            get
            {
                // Double-Checked Locking
                if (null == _assembly)
                {
                    lock (SynObject)
                    {
                        if (null == _assembly)
                        {
                            _assembly = new AssemblyHelp();
                        }
                    }
                }
                return _assembly;
            }
        }
        #endregion

        #region 动态调用 DLL 方法 +object Invoke(string nameSpace, string className, string methodName)
        /// <summary>
        /// 动态调用 DLL 方法
        /// </summary>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public object Invoke(string nameSpace, string className, string methodName)
        {
            string files = FULLPATH;
            string assems = nameSpace + "." + className;

            Assembly asm = GetAsm(files);
            var type = asm.GetType(assems);
            var instance = asm.CreateInstance(assems);

            var method = type.GetMethod(methodName);
            object obj = method.Invoke(instance, null);
            return obj;
        }

        #endregion

        #region 动态调用 DLL返回类型 +Type GetDllType(string name,string nameSpace, string className)
        /// <summary>
        /// 动态调用 DLL 方法
        /// </summary>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public Type GetDllType(string name, string nameSpace, string className)
        {
            string files = FULLPATH + @"\" + name + @"\" + nameSpace + ".dll";
            string assems = nameSpace + "." + className;

            Assembly asm = GetAsm(files);
            Type type = asm.GetType(assems);

            return type;
        }

        #endregion

        #region 动态调用 DLL返回所有名称 +List<string> GetDllTypeNames(string name, string nameSpace)
        /// <summary>
        /// 动态调用 DLL 方法
        /// </summary>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public static List<Type> GetDllTypeNames(string name, string nameSpace)
        {
            List<Type> types = new List<Type>();
            string files = FULLPATH + @"\" + name + @"\" + nameSpace + ".dll";

            Assembly asm = GetAsm(files);
            foreach (Type type in asm.GetTypes())
            {
                types.Add(type);   //显示该dll下所有的类
            }
            return types;
        }

        #endregion

        #region 动态调用 DLL返回类型 +Type GetDllTypeI(string name,string nameSpace, string className)
        /// <summary>
        /// 动态调用 DLL 方法
        /// </summary>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public object GetDllTypeI(string name, string nameSpace, string className)
        {
            string files = FULLPATH + @"\" + name + @"\" + nameSpace + ".dll";
            string assems = nameSpace + "." + className;

            Assembly asmT = GetAsm(files);
            object asm = asmT.CreateInstance(assems);

            return asm;
        }

        #endregion


        private static Assembly GetAsmOld(string assemblyFile)
        {
            Assembly asm;
            using (MemoryStream memStream = FileHelp.GetFileStream(assemblyFile))
            {
                asm = Assembly.Load(memStream.ToArray());
                memStream.Flush();
                memStream.Close();
                memStream.Dispose();
                GC.GetTotalMemory(true);
            }
            return asm;
        }
        private static Assembly GetAsm(string assemblyFile)
        {
            Assembly asm;
            byte[] fileData = File.ReadAllBytes(assemblyFile);
            asm = Assembly.Load(fileData);
            return asm;
        }

    }
}

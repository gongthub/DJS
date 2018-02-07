using System;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using DJS.Common.CommonModel;
using System.Collections.Generic;
using System.Threading;

namespace DJS.Common
{
    public class CpuMemoryHelp
    {
        private static List<CpuMemoryModel> MODELS = new List<CpuMemoryModel>();
        private static PerformanceCounter cpu;
        /// <summary>
        /// 数据保存分钟数
        /// </summary>
        private const int SAVEM = 1;
        //public static ComputerInfo cif;  

        [DllImport("kernel32")]
        public static extern void GetSystemDirectory(StringBuilder SysDir, int count);
        [DllImport("kernel32")]
        public static extern void GetSystemInfo(ref  CPU_INFO cpuinfo);
        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref  MEMORY_INFO meminfo);
        [DllImport("kernel32")]
        public static extern void GetSystemTime(ref  SYSTEMTIME_INFO stinfo);

        public static CpuMemoryModel GetCpuMemory()
        {
            CpuMemoryModel model = new CpuMemoryModel();
            try
            {
                model.Times = DateTime.Now;

                cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                // cif = new ComputerInfo();  
                MEMORY_INFO MemInfo;
                MemInfo = new MEMORY_INFO();
                //CPU_INFO cpuInfo = new CPU_INFO();

                //GetSystemInfo(ref  cpuInfo);

                GlobalMemoryStatus(ref  MemInfo);
                //Console.WriteLine(MemInfo.dwMemoryLoad.ToString() + "%的内存正在使用");
                var percentage = cpu.NextValue();
                //Console.WriteLine(percentage + "%的CPU正在使用\n");
                model.cpu = cpu.NextValue();
                model.memory = MemInfo.dwMemoryLoad;
            }
            catch
            {
            }
            return model;
        }

        public static List<CpuMemoryModel> GetCpuMemorys()
        {
            List<CpuMemoryModel> models = new List<CpuMemoryModel>();
            if (MODELS != null)
            {
                models = MODELS;
            }
            return models;
        }

        public static void InitGetCpuMemorys()
        {
            List<CpuMemoryModel> models = new List<CpuMemoryModel>();

            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    if (MODELS == null)
                    {
                        MODELS = new List<CpuMemoryModel>();
                    }
                    CpuMemoryModel model = GetCpuMemory();
                    MODELS.Add(model);
                    DateTime endTime = DateTime.Now.AddMinutes(-SAVEM);
                    MODELS = MODELS.FindAll(m => m.Times > endTime);
                    Thread.Sleep(1000);
                }
            });
            thread.Start();
        }
    }
    //定义CPU的信息结构    
    [StructLayout(LayoutKind.Sequential)]
    public struct CPU_INFO
    {
        public uint dwOemId;
        public uint dwPageSize;
        public uint lpMinimumApplicationAddress;
        public uint lpMaximumApplicationAddress;
        public uint dwActiveProcessorMask;
        public uint dwNumberOfProcessors;
        public uint dwProcessorType;
        public uint dwAllocationGranularity;
        public uint dwProcessorLevel;
        public uint dwProcessorRevision;
    }
    //定义内存的信息结构    
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_INFO
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public uint dwTotalPhys;
        public uint dwAvailPhys;
        public uint dwTotalPageFile;
        public uint dwAvailPageFile;
        public uint dwTotalVirtual;
        public uint dwAvailVirtual;
    }
    //定义系统时间的信息结构    
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME_INFO
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
    }
}

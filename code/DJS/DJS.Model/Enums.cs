using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public class Enums
    {
        /// <summary>
        /// 任务运行时间类型
        /// </summary>
        public enum TimeType
        {
            [Description("周期性")]
            Periodicity = 0,
            [Description("一次性")]
            Disposable = 1
        }

        /// <summary>
        /// 任务类型
        /// </summary>
        public enum JobType
        {
            [Description(".NET DLL")]
            DLL = 0

        }

        /// <summary>
        /// 触发器状态
        /// </summary>
        public enum TriggerState
        {
            [Description("正常")]
            Normal = 0,
            [Description("暂停")]
            Paused = 1,
            [Description("完成")]
            Complete = 2,
            [Description("错误")]
            Error = 3,
            [Description("受阻")]
            Blocked = 4,
            [Description("无")]
            None = 5
        }
        /// <summary>
        /// 日志类型
        /// </summary>
        public enum LogType
        {
            [Description("正常")]
            Normal = 0,
            [Description("错误")]
            Error = 1
        }

        /// <summary>
        /// 日志存储类型
        /// </summary>
        public enum LogFileType
        {
            [Description("Redis")]
            Redis = 0,
            [Description("File")]
            File = 1
        }
    }
}

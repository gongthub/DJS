using DJS.Core.CPlatform.Server.Utilities;

namespace DJS.Core.CPlatform.Messages
{
    /// <summary>
    /// 远程调用结果消息。
    /// </summary>
    public class RemoteInvokeResultMessage
    {
        /// <summary>
        /// 异常消息。
        /// </summary>
        public string ExceptionMessage { get; set; }
        /// <summary>
        /// 返回消息类型。
        /// </summary>
        public RemoteInvokeResultType remoteInvokeResultType { get; set; }

        /// <summary>
        /// 结果内容。
        /// </summary>
        public object Result { get; set; }
    }
}
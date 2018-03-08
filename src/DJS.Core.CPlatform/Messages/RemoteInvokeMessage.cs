﻿using System.Collections.Generic;

namespace DJS.Core.CPlatform.Messages
{
    /// <summary>
    /// 远程调用消息。
    /// </summary>
    public class RemoteInvokeMessage
    {
        /// <summary>
        /// 服务Id。
        /// </summary>
        public string ServiceId { get; set; }

        public string Token { get; set; }

        public bool DecodeJOject { get; set; }

        public string ServiceKey { get; set; }
        /// <summary>
        /// 服务参数。
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }
    }
}
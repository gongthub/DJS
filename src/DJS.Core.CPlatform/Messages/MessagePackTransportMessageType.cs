using DJS.Core.CPlatform.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.CPlatform.Messages
{
    public class MessagePackTransportMessageType
    { 
        public static string remoteInvokeResultMessageTypeName= typeof(RemoteInvokeResultMessage).FullName;

        public static string remoteInvokeMessageTypeName = typeof(RemoteInvokeMessage).FullName;
    }
}

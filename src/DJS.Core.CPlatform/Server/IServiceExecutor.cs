﻿using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Transport;
using System.Threading.Tasks;

namespace DJS.Core.CPlatform.Server
{
    /// <summary>
    /// 一个抽象的服务执行器。
    /// </summary>
    public interface IServiceExecutor
    {
        /// <summary>
        /// 执行。
        /// </summary>
        /// <param name="sender">消息发送者。</param>
        /// <param name="message">调用消息。</param>
        Task ExecuteAsync(IMessageSender sender, IServerHostProvider serverHostProvider, TransportMessage message);
        /// <summary>
        /// 执行。
        /// </summary>
        /// <param name="sender">消息发送者。</param>
        /// <param name="message">调用消息。</param>
        Task ExecuteAsync(IMessageSender sender,TransportMessage message);
       
    }
}
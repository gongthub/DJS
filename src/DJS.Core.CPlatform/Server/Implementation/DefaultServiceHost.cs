﻿using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Transport;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DJS.Core.CPlatform.Server.Implementation
{
    /// <summary>
    /// 一个默认的服务主机。
    /// </summary>
    public class DefaultServiceHost : ServiceHostAbstract
    {
        #region Field

        private readonly Func<EndPoint, Task<IMessageListener>> _messageListenerFactory;
        private IMessageListener _serverMessageListener;
        private IServiceExecutor _serviceExecutor;
        private IServerHostProvider _serverHostProvider;

        #endregion Field

        public DefaultServiceHost(Func<EndPoint, Task<IMessageListener>> messageListenerFactory, IServiceExecutor serviceExecutor, IServerHostProvider serverHostProvider) : base(serviceExecutor, serverHostProvider)
        {
            _messageListenerFactory = messageListenerFactory;
            _serviceExecutor = serviceExecutor;
            _serverHostProvider = serverHostProvider;
        }
        public DefaultServiceHost(Func<EndPoint, Task<IMessageListener>> messageListenerFactory, IServiceExecutor serviceExecutor) : base(serviceExecutor)
        {
            _messageListenerFactory = messageListenerFactory;
            _serviceExecutor = serviceExecutor;
        }

        #region Overrides of ServiceHostAbstract

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public override void Dispose()
        {
            (_serverMessageListener as IDisposable)?.Dispose();
        }

        /// <summary>
        /// 启动主机。
        /// </summary>
        /// <param name="endPoint">主机终结点。</param>
        /// <returns>一个任务。</returns>
        public override async Task StartAsync(EndPoint endPoint)
        {
            if (_serverMessageListener != null)
                return;
            _serverMessageListener = await _messageListenerFactory(endPoint);
            _serverMessageListener.Received += async (sender, message) =>
            {
                await Task.Run(() =>
                {
                    MessageListener.OnReceived(sender, message);
                });
            };
        }

        #endregion Overrides of ServiceHostAbstract
    }
}
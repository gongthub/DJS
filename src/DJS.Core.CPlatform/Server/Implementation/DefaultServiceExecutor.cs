﻿using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Server.Utilities;
using DJS.Core.CPlatform.Transport;
using System;
using System.Threading.Tasks;

namespace DJS.Core.CPlatform.Server.Implementation
{
    public class DefaultServiceExecutor : IServiceExecutor
    {
        #region Field

        #endregion Field

        #region Constructor

        public DefaultServiceExecutor()
        {
        }

        #endregion Constructor

        #region Implementation of IServiceExecutor

        /// <summary>
        /// 执行。
        /// </summary>
        /// <param name="sender">消息发送者。</param>
        /// <param name="message">调用消息。</param>
        public async Task ExecuteAsync(IMessageSender sender, TransportMessage message)
        {

            Console.WriteLine("接收到消息。");

            if (!message.IsInvokeMessage())
                return;

            RemoteInvokeMessage remoteInvokeMessage;
            try
            {
                remoteInvokeMessage = message.GetContent<RemoteInvokeMessage>();
            }
            catch (Exception exception)
            {
                Console.WriteLine("将接收到的消息反序列化成 TransportMessage<RemoteInvokeMessage> 时发送了错误。");
                return;
            }

            Console.WriteLine("准备执行本地逻辑。");

            var resultMessage = new RemoteInvokeResultMessage();

            ////是否需要等待执行。
            //if (entry.Descriptor.WaitExecution())
            //{
            //执行本地代码。
            //await LocalExecuteAsync(sender, remoteInvokeMessage, resultMessage);
            //向客户端发送调用结果。
            //await SendRemoteInvokeResult(sender, message.Id, resultMessage);
            //}
            //else
            //{
            //    //通知客户端已接收到消息。
            //    await SendRemoteInvokeResult(sender, message.Id, resultMessage);
            //    //确保新起一个线程执行，不堵塞当前线程。
            //    await Task.Factory.StartNew(async () =>
            //    {
            //        //执行本地代码。
            //        //await   LocalExecuteAsync(entry, remoteInvokeMessage, resultMessage);
            //    }, TaskCreationOptions.LongRunning);
            //}
        }

        /// <summary>
        /// 执行。
        /// </summary>
        /// <param name="sender">消息发送者。</param>
        /// <param name="message">调用消息。</param>
        public async Task ExecuteAsync(IMessageSender sender, IServerHostProvider serverHostProvider, TransportMessage message)
        {

            Console.WriteLine("接收到消息。");

            if (!message.IsInvokeMessage())
                return;

            RemoteInvokeMessage remoteInvokeMessage;
            try
            {
                remoteInvokeMessage = message.GetContent<RemoteInvokeMessage>();
            }
            catch (Exception exception)
            {
                Console.WriteLine("将接收到的消息反序列化成 TransportMessage<RemoteInvokeMessage> 时发送了错误。");
                return;
            }

            Console.WriteLine("准备执行本地逻辑。");

            var resultMessage = new RemoteInvokeResultMessage();

            ////是否需要等待执行。
            //if (entry.Descriptor.WaitExecution())
            //{
            //执行本地代码。
            await LocalExecuteAsync(sender, serverHostProvider, remoteInvokeMessage, resultMessage);
            //向客户端发送调用结果。
            await SendRemoteInvokeResult(sender, message.Id, resultMessage);

            //}
            //else
            //{
            //    //通知客户端已接收到消息。
            //    await SendRemoteInvokeResult(sender, message.Id, resultMessage);
            //    //确保新起一个线程执行，不堵塞当前线程。
            //    await Task.Factory.StartNew(async () =>
            //    {
            //        //执行本地代码。
            //        //await   LocalExecuteAsync(entry, remoteInvokeMessage, resultMessage);
            //    }, TaskCreationOptions.LongRunning);
            //}
        }
        #endregion Implementation of IServiceExecutor

        #region Private Method


        private async Task LocalExecuteAsync(IMessageSender sender, IServerHostProvider serverHostProvider, RemoteInvokeMessage remoteInvokeMessage, RemoteInvokeResultMessage resultMessage)
        {
            try
            {
                Console.WriteLine("准备处理请求消息。");

                Console.WriteLine("请求消息类型：" + remoteInvokeMessage.InvokeType);

                switch (remoteInvokeMessage.InvokeType)
                {
                    case RemoteInvokeType.SubScriptionScheduler:
                        resultMessage.remoteInvokeResultType = RemoteInvokeResultType.Default;
                        await Task.Run(() =>
                        {
                            serverHostProvider.SetSubSchedulerClient(sender);
                        });
                        resultMessage.Result = "订阅调度器成功！";
                        break;
                    case RemoteInvokeType.SubScriptionExecute:
                        resultMessage.remoteInvokeResultType = RemoteInvokeResultType.Default;
                        await Task.Run(() =>
                        {
                            serverHostProvider.SetSubExecuteClient(sender);
                        });
                        resultMessage.Result = "订阅执行器成功！";
                        break;
                    case RemoteInvokeType.TriggerJob:
                        resultMessage.remoteInvokeResultType = RemoteInvokeResultType.Default;
                        await Task.Run(() =>
                        {
                            serverHostProvider.PublishExecuteJobs(remoteInvokeMessage.ServiceId);
                        });
                        resultMessage.Result = "触发执行器任务成功！";
                        break;
                    case RemoteInvokeType.SetExecuteJob:
                        resultMessage.remoteInvokeResultType = RemoteInvokeResultType.Default;
                        await Task.Run(() =>
                        {
                            serverHostProvider.PublishExecuteJobFiles(remoteInvokeMessage.ServiceId);
                        });
                        resultMessage.Result = "发布执行器任务信息成功！";
                        break;
                    case RemoteInvokeType.Heartbeat:
                        resultMessage.remoteInvokeResultType = RemoteInvokeResultType.Default;
                        resultMessage.Result = "心跳成功！";
                        break;
                }
                Console.WriteLine("请求消息处理成功。");
            }
            catch (Exception exception)
            {
                resultMessage.ExceptionMessage = GetExceptionMessage(exception);
                Console.WriteLine("发送响应消息时候发生了异常。");
            }
        }

        private async Task SendRemoteInvokeResult(IMessageSender sender, string messageId, RemoteInvokeResultMessage resultMessage)
        {
            try
            {
                Console.WriteLine("准备发送响应消息。");

                await sender.SendAndFlushAsync(TransportMessage.CreateInvokeResultMessage(messageId, resultMessage));
                Console.WriteLine("响应消息发送成功。");
            }
            catch (Exception exception)
            {
                resultMessage.ExceptionMessage = GetExceptionMessage(exception);
                Console.WriteLine("发送响应消息时候发生了异常。");
            }
        }

        private static string GetExceptionMessage(Exception exception)
        {
            if (exception == null)
                return string.Empty;

            var message = exception.Message;
            if (exception.InnerException != null)
            {
                message += "|InnerException:" + GetExceptionMessage(exception.InnerException);
            }
            return message;
        }

        #endregion Private Method
    }
}
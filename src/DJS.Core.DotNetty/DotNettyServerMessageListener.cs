﻿using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Server;
using DJS.Core.CPlatform.Transport;
using DJS.Core.CPlatform.Transport.Codec;
using DJS.Core.DotNetty.Adaper;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace DJS.Core.DotNetty
{
    public class DotNettyServerMessageListener : IMessageListener, IDisposable
    {
        #region Field

        private readonly ITransportMessageDecoder _transportMessageDecoder;
        private readonly ITransportMessageEncoder _transportMessageEncoder;
        private readonly IServerHostProvider _serverHostProvider;
        private IChannel _channel;

        #endregion Field

        #region Constructor

        public DotNettyServerMessageListener(ITransportMessageCodecFactory codecFactory)
        {
            _transportMessageEncoder = codecFactory.GetEncoder();
            _transportMessageDecoder = codecFactory.GetDecoder();
        }
        public DotNettyServerMessageListener(ITransportMessageCodecFactory codecFactory, IServerHostProvider serverHostProvider)
        {
            _transportMessageEncoder = codecFactory.GetEncoder();
            _transportMessageDecoder = codecFactory.GetDecoder();
            _serverHostProvider = serverHostProvider;
        }

        #endregion Constructor

        #region Implementation of IMessageListener

        public event ReceivedDelegate Received;

        /// <summary>
        /// 触发接收到消息事件。
        /// </summary>
        /// <param name="sender">消息发送者。</param>
        /// <param name="message">接收到的消息。</param>
        /// <returns>一个任务。</returns>
        public async Task OnReceived(IMessageSender sender, TransportMessage message)
        {
            if (Received == null)
                return;
            await Received(sender, message);
        }
        #endregion Implementation of IMessageListener

        public async Task StartAsync(EndPoint endPoint)
        {
            Debug.WriteLine($"准备启动服务主机，监听地址：{endPoint}。");

            var bossGroup = new MultithreadEventLoopGroup();
            var workerGroup = new MultithreadEventLoopGroup(4);
            var bootstrap = new ServerBootstrap();
            bootstrap
            .Group(bossGroup, workerGroup)
            .Channel<TcpServerSocketChannel>()
            .Option(ChannelOption.SoBacklog, 100)
            .ChildOption(ChannelOption.Allocator, PooledByteBufferAllocator.Default)
            .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
            {
                var pipeline = channel.Pipeline;
                pipeline.AddLast(new LengthFieldPrepender(4));
                pipeline.AddLast(new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 4, 0, 4));
                pipeline.AddLast(new TransportMessageChannelHandlerAdapter(_transportMessageDecoder));
                pipeline.AddLast(new ServerHandler(async (contenxt, message) =>
                {
                    var sender = new DotNettyServerMessageSender(_transportMessageEncoder, contenxt);
                    sender.ClientId = contenxt.Channel.Id.ToString();
                    await OnReceived(sender, message);
                }));
            }));
            _channel = await bootstrap.BindAsync(endPoint);
            Debug.WriteLine($"服务主机启动成功，监听地址：{endPoint}。");
        }


        #region Implementation of IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Task.Run(async () =>
            {
                await _channel.DisconnectAsync();
            }).Wait();
        }

        #endregion Implementation of IDisposable

        #region Help Class

        private class ServerHandler : ChannelHandlerAdapter
        {
            private readonly Action<IChannelHandlerContext, TransportMessage> _readAction;

            public ServerHandler(Action<IChannelHandlerContext, TransportMessage> readAction)
            {
                _readAction = readAction;
            }

            #region Overrides of ChannelHandlerAdapter

            public override void ChannelRead(IChannelHandlerContext context, object message)
            {
                Task.Run(() =>
                {
                    var transportMessage = (TransportMessage)message;
                    _readAction(context, transportMessage);
                });
            }

            public override void ChannelReadComplete(IChannelHandlerContext context)
            {
                context.Flush();
            }

            public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
            {
                Debug.WriteLine(exception, $"与服务器：{context.Channel.RemoteAddress}通信时发送了错误。");
            }

            #endregion Overrides of ChannelHandlerAdapter
        }

        #endregion Help Class
    }
}
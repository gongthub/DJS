using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Transport;
using DJS.Core.CPlatform.Transport.Codec;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DJS.Core.DotNetty
{
    /// <summary>
    /// 基于DotNetty客户端的消息发送者。
    /// </summary>
    public class DotNettyMessageClientSender : DotNettyMessageSender, IMessageSender, IDisposable
    {
        private static List<IChannel> SUBSCHEDULERLST = new List<IChannel>();

        private readonly IChannel _channel;

        public string ClientId { get; set; }

        public DotNettyMessageClientSender(ITransportMessageEncoder transportMessageEncoder, IChannel channel) : base(transportMessageEncoder)
        {
            _channel = channel;
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

        #region Implementation of IMessageSender

        /// <summary>
        /// 发送消息。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <returns>一个任务。</returns>
        public async Task SendAsync(TransportMessage message)
        {
            var buffer = GetByteBuffer(message);
            await _channel.WriteAndFlushAsync(buffer);
        }

        /// <summary>
        /// 发送消息并清空缓冲区。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <returns>一个任务。</returns>
        public async Task SendAndFlushAsync(TransportMessage message)
        {
            var buffer = GetByteBuffer(message);
            await _channel.WriteAndFlushAsync(buffer);
        }

        #endregion Implementation of IMessageSender
    }
}

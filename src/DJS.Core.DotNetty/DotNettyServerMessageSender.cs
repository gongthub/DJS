using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Transport;
using DJS.Core.CPlatform.Transport.Codec;
using DotNetty.Transport.Channels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DJS.Core.DotNetty
{

    /// <summary>
    /// 基于DotNetty服务端的消息发送者。
    /// </summary>
    public class DotNettyServerMessageSender : DotNettyMessageSender, IMessageSender
    {
        private static List<IChannelHandlerContext> SUBSCHEDULERLST = new List<IChannelHandlerContext>();

        private readonly IChannelHandlerContext _context;

        public string ClientId { get; set; }

        public DotNettyServerMessageSender(ITransportMessageEncoder transportMessageEncoder) : base(transportMessageEncoder)
        {
        }
        public DotNettyServerMessageSender(ITransportMessageEncoder transportMessageEncoder, IChannelHandlerContext context) : base(transportMessageEncoder)
        {
            _context = context;
        }

        #region Implementation of IMessageSender

        /// <summary>
        /// 发送消息。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <returns>一个任务。</returns>
        public Task SendAsync(TransportMessage message)
        {
            var buffer = GetByteBuffer(message);
            return _context.WriteAsync(buffer);
        }

        /// <summary>
        /// 发送消息并清空缓冲区。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <returns>一个任务。</returns>
        public Task SendAndFlushAsync(TransportMessage message)
        {
            var buffer = GetByteBuffer(message);
            return _context.WriteAndFlushAsync(buffer);
        }
        #endregion Implementation of IMessageSender
    }
}

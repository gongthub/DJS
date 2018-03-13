using DJS.Core.CPlatform.Messages;
using DJS.Core.CPlatform.Transport.Codec;
using DotNetty.Buffers;

namespace DJS.Core.DotNetty
{
    /// <summary>
    /// 基于DotNetty的消息发送者基类。
    /// </summary>
    public abstract class DotNettyMessageSender
    {
        private readonly ITransportMessageEncoder _transportMessageEncoder;

        protected DotNettyMessageSender(ITransportMessageEncoder transportMessageEncoder)
        {
            _transportMessageEncoder = transportMessageEncoder;
        }

        protected IByteBuffer GetByteBuffer(TransportMessage message)
        {
            var data = _transportMessageEncoder.Encode(message);
            //var buffer = PooledByteBufferAllocator.Default.Buffer();
            return Unpooled.WrappedBuffer(data);
        }
    }

}
using DJS.Core.CPlatform.Messages;

namespace DJS.Core.CPlatform.Transport.Codec
{
    public interface ITransportMessageDecoder
    {
        TransportMessage Decode(byte[] data);
    }
}
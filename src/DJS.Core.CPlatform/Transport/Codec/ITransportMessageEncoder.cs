using DJS.Core.CPlatform.Messages;

namespace DJS.Core.CPlatform.Transport.Codec
{
    public interface ITransportMessageEncoder
    {
        byte[] Encode(TransportMessage message);
    }
}
using System.Threading.Tasks;

namespace Common.Connection
{
    public interface ITcpSocketHandler
    {
        void Listen(int port);

        Task AcceptAsync();

        void Connect(int remotePort);
        
        Task<byte[]> ReceiveAsync();

        void Send(byte[] message);

        void CloseWorkingSocket();

        void CloseListening();
    }
}

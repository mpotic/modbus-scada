using System.Threading.Tasks;

namespace Common.Connection
{
    public interface ITcpSocketHandler
    {
        bool IsConnected { get; }

        void Listen(int port);

        Task AcceptAsync();

        void Connect(int remotePort);
        
        Task<byte[]> ReceiveAsync();

        void Send(byte[] message);

        Task<int> SendAsync(byte[] message);

        void CloseWorkingSocket();

        void CloseListening();
    }
}

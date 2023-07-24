using System.Threading.Tasks;

namespace Common.Connection
{
    public interface ITcpSocketHandler
    {
        bool IsConnected { get; }

        void Listen(int port);

        Task AcceptAsync();

        Task ConnectAsync(int remotePort);

        Task ConnectAsync(int remotePort, int localPort);

		Task<byte[]> ReceiveAsync();

        void Send(byte[] message);

        void CloseAndUnbindWorkingSocket();

        void CloseListening();
    }
}

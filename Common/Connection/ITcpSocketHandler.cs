using System.Threading.Tasks;

namespace Common.Connection
{
    /// <summary>
    /// Connects a socket using a TCP connection, listens and accepts connections and manages sending and receiving.
    /// </summary>
    public interface ITcpSocketHandler
    {
        bool IsConnected { get; }

        void Listen(int port);

        Task AcceptAsync();

        Task ConnectAsync(int remotePort, int localPort);

		Task<byte[]> ReceiveAsync();

        Task<byte[]> ReceiveWithTimeout();

		void Send(byte[] message);

        void ClearReceiveBuffer();

        void CloseAndUnbindWorkingSocket();

        void CloseListening();
    }
}

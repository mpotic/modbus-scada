using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Common.Connection
{
    public class TcpSocketHandler : ITcpSocketHandler
	{
		Socket listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

		Socket workingSocket;

		public void Listen(int port)
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, port);
            listenSocket.Bind(endpoint);
            listenSocket.Listen(1);
		}

		public async Task AcceptAsync()
		{
			workingSocket = await listenSocket.AcceptAsync();
		}

		public void Connect(int remotePort)
		{
			workingSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Loopback, remotePort);
			workingSocket.Connect(remoteEndpoint);
		}

		public async Task<byte[]> ReceiveAsync()
		{
			ArraySegment<byte> receivedData = new ArraySegment<byte>(new byte[1024]);
			await workingSocket.ReceiveAsync(receivedData, SocketFlags.None);

			return receivedData.ToArray();
		}

		public void Send(byte[] message)
		{
			workingSocket.Send(message);
		}

		public void CloseWorkingSocket()
		{
			if (workingSocket.Connected)
			{
                workingSocket.Close();
            }
		}

		public void CloseListening()
        {
            bool isListening = listenSocket.Poll(0, SelectMode.SelectRead);
			if (isListening)
			{
                listenSocket.Close();
            }
		}
    }
}

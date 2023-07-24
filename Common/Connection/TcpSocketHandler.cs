using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Common.Connection
{
	public class TcpSocketHandler : ITcpSocketHandler
	{
		Socket listenSocket;

		Socket workingSocket;

		public bool IsConnected
		{
			get
			{
				if (workingSocket == null)
				{
					return false;
				}

				return workingSocket.Connected;
			}
		}

		public void Listen(int port)
		{
			listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, port);
			listenSocket.Bind(endpoint);
			listenSocket.Listen(100);
		}

		public async Task AcceptAsync()
		{
			if (workingSocket != null)
			{
				workingSocket.Close();
			}

			workingSocket = await listenSocket.AcceptAsync();
		}

		public async Task ConnectAsync(int remotePort)
		{
			CloseAndUnbindWorkingSocket();
			workingSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Loopback, remotePort);
			await workingSocket.ConnectAsync(remoteEndpoint);
		}

		public async Task ConnectAsync(int remotePort, int localPort)
		{
			CloseAndUnbindWorkingSocket();
			workingSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

			if (!workingSocket.IsBound)
			{
				IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Loopback, localPort);
				workingSocket.Bind(localEndpoint);
			}

			IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Loopback, remotePort);
			await workingSocket.ConnectAsync(remoteEndpoint);
		}

		public async Task<byte[]> ReceiveAsync()
		{
			ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[1024]);
			await workingSocket.ReceiveAsync(receiveBuffer, SocketFlags.None);

			return receiveBuffer.ToArray();
		}

		public void Send(byte[] message)
		{
			workingSocket.Send(message);
		}

		public void CloseAndUnbindWorkingSocket()
		{
			if (workingSocket == null)
			{
				return;
			}

			if (workingSocket.Connected)
			{
				workingSocket.Shutdown(SocketShutdown.Both);
			}

			workingSocket.Close();
		}

		public void CloseListening()
		{
			if (listenSocket == null)
			{
				return;
			}

			listenSocket.Close();
		}
	}
}

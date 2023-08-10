using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Connection
{
	public class TcpSocketHandler : ITcpSocketHandler
	{
		Socket listenSocket;

		Socket workingSocket;

		int expectedPacketSize = 1024;

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
			workingSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
			workingSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Loopback, remotePort);
			await workingSocket.ConnectAsync(remoteEndpoint);
		}

		public async Task ConnectAsync(int remotePort, int localPort)
		{
			workingSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
			workingSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Loopback, localPort);
			workingSocket.Bind(localEndpoint);
			IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Loopback, remotePort);
			await workingSocket.ConnectAsync(remoteEndpoint);
		}

		public async Task<byte[]> ReceiveAsync()
		{
			byte[] buffer = new byte[expectedPacketSize];
			int count = 0;

			do
			{
				count += await workingSocket.ReceiveAsync(new ArraySegment<byte>(buffer, count, expectedPacketSize - count), SocketFlags.None);
				
				if(count == 0)
				{
					return new byte[0];
				}
			} while (count < expectedPacketSize);

			return buffer;
		}

		public async Task<byte[]> ReceiveWithTimeout()
		{
			byte[] buffer = new byte[0];
			DateTime startTime = DateTime.UtcNow;
			TimeSpan timeout = TimeSpan.FromSeconds(3);

			while (true)
			{
				TimeSpan elapsed = DateTime.UtcNow - startTime;
				if (elapsed >= timeout)
				{
					break;
				}

				if (workingSocket.Poll(1000, SelectMode.SelectRead))
				{
					buffer = await ReceiveAsync();
					break;
				}

				Thread.Sleep(10);
			}

			return buffer;
		}

		public void Send(byte[] message)
		{
			byte[] buffer = new byte[expectedPacketSize];
			message.Take(1024).ToArray().CopyTo(buffer, 0);
			workingSocket.Send(buffer);
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
				workingSocket.Close();
			}

			workingSocket.Dispose();
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

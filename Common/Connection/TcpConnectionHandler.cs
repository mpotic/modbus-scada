using System;
using System.Net;
using System.Net.Sockets;

namespace Common.Util
{
	public class TcpConnectionHandler : ITcpConnectionHandler, IDisposable
	{
		/// <summary>
		/// Used for listening to incoming requests.
		/// </summary>
		Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		/// <summary>
		/// Used for accepting a connection request.
		/// </summary>
		Socket acceptedSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		/// <summary>
		/// Used for initiating a connection with a remote server.
		/// </summary>
		Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		public void Listen(int port)
		{
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, port);
			listenSocket.Bind(endpoint);
			listenSocket.Listen(100);
		}

		public void Accept()
		{
			if (acceptedSocket.Connected)
			{
				acceptedSocket.Disconnect(true);
			}

			acceptedSocket = listenSocket.Accept();
		}

		public void BeginAccept()
		{
			if (acceptedSocket.Connected)
			{
				acceptedSocket.Disconnect(true);
			}

			var acceptResult = listenSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
		}

		private void AcceptCallback(IAsyncResult result)
		{
			Socket acceptedSocket = listenSocket.EndAccept(result);
		}

		public void Connect(int serverPort, int clientPort)
		{
			if (clientSocket.Connected)
			{
				clientSocket.Shutdown(SocketShutdown.Both);
				clientSocket.Disconnect(true);
			}

			if (clientSocket.IsBound)
			{
				clientSocket.Close();
				clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			}

			IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Loopback, serverPort);
			clientSocket.Bind(localEndpoint);
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, clientPort);
			clientSocket.Connect(endpoint);
		}

		public byte[] ReceiveClient()
		{
			byte[] recvBuffer = new byte[1024];
			clientSocket.Receive(recvBuffer);

			return recvBuffer;
		}

		public byte[] ReceiveAccepted()
		{
			byte[] recvBuffer = new byte[1024];
			acceptedSocket.Receive(recvBuffer);

			return recvBuffer;
		}

		public void BeginReceiveClient(AsyncCallback callback)
		{
			byte[] recvBuffer = new byte[1024];
			clientSocket.BeginReceive(recvBuffer, 0, 1024, SocketFlags.None, callback, recvBuffer);
		}

		public void SendClient(byte[] data)
		{
			clientSocket.Send(data);
		}

		public void SendAccepted(byte[] data)
		{
			acceptedSocket.Send(data);
		}

		public void Dispose()
		{
			clientSocket.Shutdown(SocketShutdown.Both);
			acceptedSocket.Shutdown(SocketShutdown.Both);
			listenSocket.Shutdown(SocketShutdown.Both);

			clientSocket.Close();
			acceptedSocket.Close();
			listenSocket.Close();
		}
	}
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Common.Connection
{
	public class TcpHandler : ITcpHandler, IDisposable
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
		Socket connectedSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		public bool IsAcceptedSocketConnected
		{
			get
			{
				return acceptedSocket.Connected;
			}
		}

		public bool IsConnectedSocketConnected
		{
			get
			{
				return connectedSocket.Connected;
			}
		}

		public void Listen(int port)
		{
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, port);
			listenSocket.Bind(endpoint);
			listenSocket.Listen(100);
		}

		public void Connect(int myPort, int remotePort)
		{
			if (connectedSocket.Connected)
			{
				connectedSocket.Shutdown(SocketShutdown.Both);
				connectedSocket.Disconnect(true);
			}

			if (connectedSocket.IsBound)
			{
				connectedSocket.Close();
				connectedSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			}

			IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Loopback, myPort);
			connectedSocket.Bind(localEndpoint);
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, remotePort);
			connectedSocket.Connect(endpoint);
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

		public async Task AcceptAsync()
		{
			await listenSocket.AcceptAsync(acceptedSocket);
		}

		private void AcceptCallback(IAsyncResult result)
		{
			Socket acceptedSocket = listenSocket.EndAccept(result);
		}

		public byte[] ReceiveConnectedSocket()
		{
			byte[] recvBuffer = new byte[1024];
			connectedSocket.Receive(recvBuffer);

			return recvBuffer;
		}

		public byte[] ReceiveAcceptedSocket()
		{
			byte[] recvBuffer = new byte[1024];
			acceptedSocket.Receive(recvBuffer);

			return recvBuffer;
		}

		public void BeginReceiveConnectedSocket(AsyncCallback callback)
		{
			byte[] recvBuffer = new byte[1024];
			connectedSocket.BeginReceive(recvBuffer, 0, 1024, SocketFlags.None, callback, recvBuffer);
		}

		public void BeginReceiveAcceptedSocket(AsyncCallback callback)
		{
			byte[] recvBuffer = new byte[1024];
			connectedSocket.BeginReceive(recvBuffer, 0, 1024, SocketFlags.None, callback, recvBuffer);
		}

		public async Task<byte[]> ReceiveConnectedSocketAsync()
		{
			byte[] dataArray = new byte[1024];
			ArraySegment<byte> receivedData = new ArraySegment<byte>(dataArray);
			await connectedSocket.ReceiveAsync(receivedData, SocketFlags.None);

			return dataArray;
		}

		public async Task<byte[]> ReceiveAcceptedSocketAsync()
		{
			byte[] dataArray = new byte[1024];
			ArraySegment<byte> receivedData = new ArraySegment<byte>(dataArray);
			await acceptedSocket.ReceiveAsync(receivedData, SocketFlags.None);

			return dataArray;
		}

		public void SendConnectedSocket(byte[] data)
		{
			connectedSocket.Send(data);
		}

		public void SendAcceptedSocket(byte[] data)
		{
			acceptedSocket.Send(data);
		}

		public void Dispose()
		{
			connectedSocket.Shutdown(SocketShutdown.Both);
			acceptedSocket.Shutdown(SocketShutdown.Both);
			listenSocket.Shutdown(SocketShutdown.Both);

			connectedSocket.Close();
			acceptedSocket.Close();
			listenSocket.Close();
		}
	}
}

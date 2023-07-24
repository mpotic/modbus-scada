using Common.Connection;
using Common.Enums;
using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.MessageHandling
{
	internal class MessageWorker : IMessageWorker
	{
		int listenPort = 21001;

		int localConnectPort = 22001;

		bool isListening = false;

		readonly int autoConnectRemotePort = 21001;

		readonly int allowedAttempts = 3;

		readonly ITcpSocketHandler acceptSocket = new TcpSocketHandler();

		readonly ITcpSocketHandler connectSocket = new TcpSocketHandler();

		readonly ITcpSerializer serializer = new TcpSerializer();

		readonly IConnectionHelper connectionHelper = new ConnectionHelper();

		readonly Dictionary<SenderCode, IMessageHandler> messageHandling;

		public MessageWorker()
		{
			messageHandling = new Dictionary<SenderCode, IMessageHandler>
			{
				{ SenderCode.Master, new MasterMessageHandler(serializer, connectSocket) },
				{ SenderCode.ProxyToMaster, new ProxyToMasterMessageHandler(serializer, acceptSocket) },
				{ SenderCode.ProxyToSlave, new ProxyToSlaveMessageHandler(serializer, acceptSocket) },
			};

			if (!connectionHelper.IsPortAvailable(listenPort))
			{
				listenPort--;
				localConnectPort--;
			}
		}

		public async void ListenAcceptAndStartReceiving()
		{
			if (!StartListening())
			{
				return;
			}

			if (!await AcceptConnection())
			{
				return;
			}

			StartReceiving(acceptSocket);
		}

		private bool StartListening()
		{
			if (isListening)
			{
				Console.WriteLine("Already listening!");

				return false;
			}

			acceptSocket.Listen(listenPort);
			Console.WriteLine($"Listening on port {listenPort}...");
			isListening = true;

			return true;
		}

		/// <summary>
		/// Accepts a socket and stops listening.
		/// </summary>
		private async Task<bool> AcceptConnection()
		{
			try
			{
				await acceptSocket.AcceptAsync();
			}
			catch (SocketException ex) when (ex.SocketErrorCode == SocketError.OperationAborted || ex.SocketErrorCode == SocketError.Shutdown)
			{
				Console.WriteLine("Accepting socket closed.");
				acceptSocket.CloseListening();
				isListening = false;

				return false;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				acceptSocket.CloseListening();
				isListening = false;

				return false;
			}

			Console.WriteLine("Connection accepted.");
			acceptSocket.CloseListening();
			isListening = false;

			return true;
		}

		public void ConnectAndStartReceiving(int remotePort)
		{
			DisconnectConnectSocket();

			try
			{
				connectSocket.ConnectAsync(remotePort, localConnectPort);
			}
			catch(Exception e) 
			{
                Console.WriteLine(e.Message);

				return;
            }

			Console.WriteLine($"Connected to port {remotePort} from {localConnectPort}.");
			StartReceiving(connectSocket);
		}

		public void Disconnect()
		{
			DisconnectAcceptSocket();
			DisconnectConnectSocket();
		}

		private void DisconnectAcceptSocket()
		{
			if (acceptSocket.IsConnected)
			{
				byte[] closeMessage = Encoding.UTF8.GetBytes(SenderCode.CloseConnection.ToString());
				acceptSocket.Send(closeMessage);
				acceptSocket.CloseAndUnbindWorkingSocket();
			}

			acceptSocket.CloseListening();

			Console.WriteLine("Disconnected accept socket.");
		}

		private void DisconnectConnectSocket()
		{
			if (connectSocket.IsConnected)
			{
				byte[] closeMessage = Encoding.UTF8.GetBytes(SenderCode.CloseConnection.ToString());
				connectSocket.Send(closeMessage);
				connectSocket.CloseAndUnbindWorkingSocket();
			}

			connectSocket.CloseListening();

			Console.WriteLine("Disconnected connect socket.");
		}

		/// <summary>
		/// Automatically try to connect to the other proxy in case port 21001 is taken, because that implies that the other proxy is 
		/// running on that port.
		/// </summary>
		public void AutoConnectAndStartReceiving()
		{
			if (listenPort == autoConnectRemotePort)
			{
				return;
			}

			int attemptCounter = 0;
			bool repeatAttempt = true;

			while (repeatAttempt)
			{
				AttemptConnection(ref attemptCounter, ref repeatAttempt, autoConnectRemotePort);

				if (attemptCounter >= allowedAttempts)
				{
					Console.WriteLine($"Maximum allowed attempts ({allowedAttempts}) reached!");
					return;
				}
			}

			Console.WriteLine($"Connected to port {autoConnectRemotePort} from {localConnectPort}.");
			StartReceiving(connectSocket);
		}

		/// <summary>
		/// Make a single connection attempt to the given port. Sets repeatAttempt to false if connection has been made.
		/// Increases the attemptCounter.
		/// </summary>
		private void AttemptConnection(ref int attemptCounter, ref bool repeatAttempt, int port)
		{
			try
			{
				connectSocket.ConnectAsync(port, localConnectPort);
				repeatAttempt = false;
			}
			catch (SocketException ex)
			{
				bool refusedOrTimeout = ex.SocketErrorCode == SocketError.ConnectionRefused
					|| ex.SocketErrorCode == SocketError.TimedOut;

				if (!refusedOrTimeout)
				{
					throw ex;
				}

				++attemptCounter;
				Console.WriteLine($"Maximum allowed attempts ({attemptCounter}) reached!");
			}
		}

		/// <summary>
		/// Starts receiving messages on a given socket handler until the socket is closed or the connection closing message is received.
		/// </summary>
		private async void StartReceiving(ITcpSocketHandler socket)
		{
			byte[] receivedMessage = null;

			while (true)
			{
				try
				{
					receivedMessage = await socket.ReceiveAsync();
					ProcessMessage(receivedMessage);
				}
				catch (SocketException ex) when (ex.SocketErrorCode == SocketError.OperationAborted || ex.SocketErrorCode == SocketError.Shutdown)
				{
					Console.WriteLine("Receiving socket closed.");
					socket.CloseAndUnbindWorkingSocket();
					socket.CloseListening();

					break;
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					socket.CloseAndUnbindWorkingSocket();
					socket.CloseListening();

					break;
				}

				if (IsConnectionClosingReceived(receivedMessage))
				{
					socket.CloseAndUnbindWorkingSocket();
					socket.CloseListening();
					Console.WriteLine("Connection terminated...");

					break;
				}
			}
		}

		/// <summary>
		/// Processes the message based on the SenderCode from header.
		/// </summary>
		private void ProcessMessage(byte[] streamMessage)
		{
			serializer.InitMessage(streamMessage);
			SenderCode senderCode = serializer.ReadSenderCodeFromHeader();

			if (messageHandling.TryGetValue(senderCode, out IMessageHandler handler))
			{
				Console.WriteLine("Received " + senderCode.ToString());
				handler.Process();
				Console.WriteLine("Processed " + senderCode.ToString());
			}
		}

		private bool IsConnectionClosingReceived(byte[] streamMessage)
		{
			if (streamMessage == null)
			{
				return false;
			}

			streamMessage = streamMessage.Where(x => x != 0).ToArray();
			string message = Encoding.UTF8.GetString(streamMessage);
			bool isSenderCodeCloseConnection = Enum.TryParse(message, out SenderCode senderCode) && senderCode == SenderCode.CloseConnection;

			if (isSenderCodeCloseConnection)
			{
				return true;
			}

			return false;
		}
	}
}

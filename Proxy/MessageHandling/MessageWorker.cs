using Common.Connection;
using Common.Enums;
using Common.Util;
using System;
using System.Collections.Generic;

namespace Proxy.MessageHandling
{
    internal class MessageWorker : IMessageWorker
	{
		private int listenPort = 21000;

		readonly ITcpSocketHandler acceptSocket = new TcpSocketHandler();

		readonly ITcpSocketHandler connectSocket = new TcpSocketHandler();

		readonly ITcpSerializer serializer = new TcpSerializer();

        IConnectionHelper connectionHelper = new ConnectionHelper();

        readonly Dictionary<SenderCode, IMessageHandler> messageHandling;

		public MessageWorker()
		{
            messageHandling = new Dictionary<SenderCode, IMessageHandler>
            {
                { SenderCode.Master, new MasterMessageHandler(serializer, connectSocket) },
                { SenderCode.ProxyToMaster, new ProxyToMasterMessageHandler(serializer, acceptSocket) },
                { SenderCode.ProxyToSlave, new ProxyToSlaveMessageHandler(serializer, acceptSocket) },
            };
        }

		public async void AcceptAndStartReceiving()
		{
			if (!connectionHelper.IsPortAvailable(listenPort))
			{
				listenPort++;
			}
			acceptSocket.Listen(listenPort);
			Console.WriteLine($"Listening on port {listenPort}...");
			await acceptSocket.AcceptAsync();
			Console.WriteLine("Connection accepted...");
			StartReceiving(acceptSocket);
		}

        public void ConnectAndStartReceiving(int remotePort)
        {
            connectSocket.Connect(remotePort);
            Console.WriteLine($"Connected to port {remotePort}...");
            StartReceiving(connectSocket);
        }

        private async void StartReceiving(ITcpSocketHandler socket)
		{
			while (true)
			{
				byte[] receivedMessage = await socket.ReceiveAsync();

				try
				{
                    ProcessMessage(receivedMessage);
                }
                catch(Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}

		private void ProcessMessage(byte[] streamMessage)
		{
			serializer.InitMessage(streamMessage);
			SenderCode senderCode = serializer.ReadSenderCodeFromHeader();
			Console.WriteLine("Received: " + senderCode.ToString() + "\nProcessing...");

			if(messageHandling.TryGetValue(senderCode, out IMessageHandler handler))
			{
				handler.Process();
			}
		}

		public void Disconnect()
		{
			acceptSocket.CloseListening();
			acceptSocket.CloseWorkingSocket();
			connectSocket.CloseListening();
			connectSocket.CloseWorkingSocket();
		}
	}
}

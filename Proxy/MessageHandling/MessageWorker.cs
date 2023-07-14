using Common.Connection;
using Common.Enums;
using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Proxy.MessageHandling
{
    internal class MessageWorker : IMessageWorker
    {
        private int listenPort = 21001;

        private int autoConnectPort = 21001;

        private int allowedAttempts = 3;

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
                listenPort--;
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

        public async void Disconnect()
        {
            if (acceptSocket.IsConnected)
            {
                byte[] closeMessage = Encoding.UTF8.GetBytes(SenderCode.CloseConnection.ToString());
                await acceptSocket.SendAsync(closeMessage);
                acceptSocket.CloseWorkingSocket();
            }

            if (connectSocket.IsConnected)
            {
                byte[] closeMessage = Encoding.UTF8.GetBytes(SenderCode.CloseConnection.ToString());
                await connectSocket.SendAsync(closeMessage);
                connectSocket.CloseWorkingSocket();
            }

            acceptSocket.CloseListening();
            connectSocket.CloseListening();
            
            Console.WriteLine("Closed connections...");
        }

        public void AutoConnectAndStartReceiving()
        {
            if(listenPort == autoConnectPort)
            {
                return;
            }

            int attemptCounter = 0;
            bool repeatAttempt = true;

            while (repeatAttempt)
            {
                AttemptConnection(ref attemptCounter, ref repeatAttempt, autoConnectPort);

                if(attemptCounter > allowedAttempts)
                {
                    Console.WriteLine("Maximum allowed attempts (" + allowedAttempts + ") reached!");
                    return;
                }
            }

            Console.WriteLine($"Connected to port {autoConnectPort}...");
            StartReceiving(connectSocket);
        }

        private void AttemptConnection(ref int attemptCounter, ref bool repeatAttempt, int port)
        {
            try
            {
                connectSocket.Connect(port);
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

                Console.WriteLine("Attempt num. " + ++attemptCounter + ".");
            }
        }

        private async void StartReceiving(ITcpSocketHandler socket)
        {
            while (true)
            {
                byte[] receivedMessage = await socket.ReceiveAsync();

                if (IsConnectionClosingReceived(receivedMessage))
                {
                    socket.CloseWorkingSocket();
                    socket.CloseListening();
                    Console.WriteLine("Connection terminated...");
                    break;
                }

                try
                {
                    ProcessMessage(receivedMessage);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void ProcessMessage(byte[] streamMessage)
        {
            serializer.InitMessage(streamMessage);
            SenderCode senderCode = serializer.ReadSenderCodeFromHeader();

            if (messageHandling.TryGetValue(senderCode, out IMessageHandler handler))
            {
                Console.WriteLine("Received: " + senderCode.ToString() + "\nProcessing...");
                handler.Process();
            }
        }

        private bool IsConnectionClosingReceived(byte[] streamMessage)
        {
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

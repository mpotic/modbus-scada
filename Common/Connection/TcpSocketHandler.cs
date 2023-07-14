using Common.Enums;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
            if (listenSocket != null)
            {
                listenSocket.Close();
            }

            listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, port);
            listenSocket.Bind(endpoint);
            listenSocket.Listen(1);
        }

        public async Task AcceptAsync()
        {
            if (workingSocket != null)
            {
                workingSocket.Close();
            }

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
            ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[1024]);

            try
            {
                await workingSocket.ReceiveAsync(receiveBuffer, SocketFlags.None);
            }
            catch (SocketException socketEx)
            {
                if(socketEx.SocketErrorCode != SocketError.OperationAborted)
                {
                    byte[] closingCode = Encoding.UTF8.GetBytes(SenderCode.CloseConnection.ToString());
                    
                    return closingCode;
                }

                throw socketEx;
            }

            return receiveBuffer.ToArray();
        }

        public void Send(byte[] message)
        {
            workingSocket.Send(message);
        }

        public async Task<int> SendAsync(byte[] message)
        {
            ArraySegment<byte> sendData = new ArraySegment<byte>(message);
            int bytesSent = await workingSocket.SendAsync(sendData, SocketFlags.None);

            return bytesSent;
        }

        public void CloseWorkingSocket()
        {
            if (workingSocket != null && workingSocket.Connected)
            {
                workingSocket.Shutdown(SocketShutdown.Both);
            }
        }

        public void CloseListening()
        {
            if (listenSocket == null)
            {
                return;
            }

            bool isListening = listenSocket.Poll(0, SelectMode.SelectRead);
            if (isListening)
            {
                listenSocket.Close();
            }
        }
    }
}

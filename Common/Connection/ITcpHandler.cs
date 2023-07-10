using System.Threading.Tasks;

namespace Common.Connection
{
	public interface ITcpHandler
	{
		/// <summary>
		/// Establishes a listening socket and puts it in listening state.
		/// </summary>
		/// <param name="port">Port on which the socket listens.</param>
		/// <returns></returns>
		void Listen(int port);

		/// <summary>
		/// Connects a socket to the server port and attempts to connect on the clients port.
		/// </summary>
		/// <param name="serverPort"></param>
		/// <param name="clientPort"></param>
		void Connect(int myPort, int remotePort);

		/// <summary>
		/// Used for accepting the connection on listening socket.
		/// </summary>
		/// <returns></returns>
		void Accept();

		/// <summary>
		/// Begins waiting for the connection and accepts it on the acceptedSocket when it is received.
		/// </summary>
		void BeginAccept();

		/// <summary>
		/// Asynchronously accept the connection attempt on acceptedSocket.
		/// </summary>
		Task AcceptAsync();

		/// <summary>
		/// Receives data on the client socket.
		/// </summary>
		/// <returns></returns>
		byte[] ReceiveConnectedSocket();

		/// <summary>
		/// Receives data on the accepted socket.
		/// </summary>
		/// <returns></returns>
		byte[] ReceiveAcceptedSocket();

		/// <summary>
		/// Asynchronous receive on accepted socket.
		/// </summary>
		/// <returns></returns>
		Task<byte[]> ReceiveAcceptedSocketAsync();

		/// <summary>
		/// Asynchronous receive on connected socket.
		/// </summary>
		/// <returns></returns>
		Task<byte[]> ReceiveConnectedSocketAsync();
		
		/// <summary>
		/// Sends data on the client socket.
		/// </summary>
		/// <returns></returns>
		void SendConnectedSocket(byte[] data);

		/// <summary>
		/// Sends data on the accepted socket.
		/// </summary>
		/// <returns></returns>
		void SendAcceptedSocket(byte[] data);
	}
}

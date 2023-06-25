namespace Common.Util
{
	public interface ITcpConnectionHandler
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
		void Connect(int serverPort, int clientPort);

		/// <summary>
		/// Used for accepting the connection on listening socket.
		/// </summary>
		/// <returns></returns>
		void Accept();

		/// <summary>
		/// Begins waiting for the connection and accepts it on the acceptedSocket when it is received. Non-blocking.
		/// </summary>
		void BeginAccept();

		/// <summary>
		/// Receives data on the client socket.
		/// </summary>
		/// <returns></returns>
		byte[] ReceiveClient();

		/// <summary>
		/// Receives data on the accepted socket.
		/// </summary>
		/// <returns></returns>
		byte[] ReceiveAccepted();

		/// <summary>
		/// Sends data on the client socket.
		/// </summary>
		/// <returns></returns>
		void SendClient(byte[] data);

		/// <summary>
		/// Sends data on the accepted socket.
		/// </summary>
		/// <returns></returns>
		void SendAccepted(byte[] data);
	}
}

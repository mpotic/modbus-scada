namespace Common.ActionDto
{
	public class ConnectionParams : IConnectionParams
	{
		public ConnectionParams(int clientPort, int serverPort)
		{
			ClientPort = clientPort;
			ServerPort = serverPort;
		}

		public int ServerPort { get; set; }

		public int ClientPort { get; set; }
	}
}

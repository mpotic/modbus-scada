namespace Common.DTO
{
	public class ConnectionParams : IConnectionParams
	{
		public ConnectionParams(int clientPort, int serverPort)
		{
			RemotePort = clientPort;
			LocalPort = serverPort;
		}

        public ConnectionParams(int clientPort)
        {
            RemotePort = clientPort;
			LocalPort = -1;
        }

        public int LocalPort { get; set; }

		public int RemotePort { get; set; }
	}
}

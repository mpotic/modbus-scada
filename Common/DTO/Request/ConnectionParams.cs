using Common.Enums;

namespace Common.DTO
{
	public sealed class ConnectionParams : IConnectionParams
	{
		public ConnectionParams() 
		{
		}	

		public ConnectionParams(int clientPort, int serverPort, ServiceTypeCode serviceType)
		{
			RemotePort = clientPort;
			LocalPort = serverPort;
			ServiceType = serviceType;
		}

		public ConnectionParams(int clientPort, int serverPort)
		{
			RemotePort = clientPort;
			LocalPort = serverPort;
			ServiceType = ServiceTypeCode.ModbusService;
		}

        public ConnectionParams(int clientPort)
        {
            RemotePort = clientPort;
			LocalPort = -1;
			ServiceType = ServiceTypeCode.ModbusService;
        }

		public ServiceTypeCode ServiceType { get; set; }

		public int LocalPort { get; set; }

		public int RemotePort { get; set; }
	}
}

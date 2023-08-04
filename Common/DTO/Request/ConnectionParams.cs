using Common.Enums;

namespace Common.DTO
{
	public sealed class ConnectionParams : IConnectionParams
	{
		public ConnectionParams(ServiceTypeCode serviceType)
		{
			ServiceType = serviceType;
		}

		public ConnectionParams(int remotePort, int localPort, ServiceTypeCode serviceType)
		{
			RemotePort = remotePort;
			LocalPort = localPort;
			ServiceType = serviceType;
		}

		public ConnectionParams(int remotePort, int localPort)
		{
			RemotePort = remotePort;
			LocalPort = localPort;
			ServiceType = ServiceTypeCode.ModbusService;
		}

        public ConnectionParams(int localPort)
        {
            LocalPort = localPort;
			ServiceType = ServiceTypeCode.ModbusService;
        }

		public ServiceTypeCode ServiceType { get; set; }

		public int LocalPort { get; set; }

		public int RemotePort { get; set; }
	}
}

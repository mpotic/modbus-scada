using ModbusService;

namespace Proxy.Connections
{
	internal class ModbusConnection : IModbusConnection
	{
		public ModbusConnection(IModbusServiceHandler serviceHandler) 
		{
			Rtu = serviceHandler.RtuServiceApi;
			Connection = serviceHandler.ConnectionServiceApi;
		}

		public IRtuServiceApi Rtu { get; }

		public IConnectionServiceApi Connection { get; }
	}
}

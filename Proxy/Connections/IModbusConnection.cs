using ModbusService;

namespace Proxy.Connections
{
	internal interface IModbusConnection : IConnection
	{
		IRtuServiceApi Rtu { get; }

		IConnectionServiceApi Connection { get; }
	}
}

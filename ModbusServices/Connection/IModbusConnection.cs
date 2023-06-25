using NModbus;

namespace ModbusServices.Connection
{
	interface IModbusConnection : IConnection
	{
		IModbusMaster ModbusMaster { get; set; }
	}
}

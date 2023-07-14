using NModbus;

namespace ModbusServices.Connection
{
	internal interface IModbusConnection : IConnection
	{
		IModbusMaster ModbusMaster { get; set; }
	}
}

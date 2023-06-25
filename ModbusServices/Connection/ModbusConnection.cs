using NModbus;

namespace ModbusServices.Connection
{
	class ModbusConnection : IModbusConnection
	{
		public IModbusMaster ModbusMaster { get; set; }
	}
}

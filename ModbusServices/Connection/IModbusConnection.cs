using NModbus;
using System.Net.Sockets;

namespace ModbusServices.Connection
{
	internal interface IModbusConnection : IConnection
	{
		IModbusMaster ModbusMaster { get; set; }

		TcpClient ClientConnection { get; set; }
	}
}

using NModbus;
using System.Net.Sockets;

namespace ModbusService
{
	internal interface IConnectionHandle
	{
		IModbusMaster ModbusMaster { get; set; }

		TcpClient TcpClient { get; set; }

		IConnectionStatus Status { get; set; }
	}
}

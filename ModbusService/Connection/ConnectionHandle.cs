using NModbus;
using System.Net.Sockets;

namespace ModbusService
{
	internal class ConnectionHandle : IConnectionHandle
	{
		public IModbusMaster ModbusMaster { get; set; }

		public TcpClient TcpClient { get; set; }

		public IConnectionStatus Status { get; set; }
	}
}

using Common.Util;

namespace ModbusServices.Connection
{
	class StandardConnection : IStandardConnection
	{
		public ITcpConnectionHandler Connection { get; set; } = new TcpConnectionHandler();
	}
}

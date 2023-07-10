using Common.Connection;

namespace ModbusServices.Connection
{
    class StandardConnection : IStandardConnection
	{
		public ITcpSocketHandler Connection { get; set; } = new TcpSocketHandler();
	}
}

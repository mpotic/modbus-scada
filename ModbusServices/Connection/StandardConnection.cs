using Common.Connection;

namespace ModbusServices.Connection
{
    internal class StandardConnection : IStandardConnection
	{
		public ITcpSocketHandler Connection { get; set; }
	}
}

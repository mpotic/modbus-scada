using Common.Util;

namespace ModbusServices.Connection
{
	interface IStandardConnection : IConnection
	{
		ITcpConnectionHandler Connection { get; set; }
	}
}

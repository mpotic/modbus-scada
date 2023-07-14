using Common.Connection;

namespace ModbusServices.Connection
{
    internal interface IStandardConnection : IConnection
	{
		ITcpSocketHandler Connection { get; set; }
	}
}

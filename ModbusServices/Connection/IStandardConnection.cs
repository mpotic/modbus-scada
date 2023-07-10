using Common.Connection;

namespace ModbusServices.Connection
{
    interface IStandardConnection : IConnection
	{
		ITcpSocketHandler Connection { get; set; }
	}
}

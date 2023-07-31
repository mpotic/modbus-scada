using Common.Connection;

namespace MasterServices.Connection
{
    internal interface IStandardConnection : IConnection
	{
		ITcpSocketHandler Connection { get; set; }
	}
}

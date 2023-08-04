using TcpService;

namespace Proxy.Connections
{
	internal interface ITcpConnection : IConnection
	{
		IConnectionApi Connection { get; }	

		ICommunicationApi Communication { get; }
	}
}

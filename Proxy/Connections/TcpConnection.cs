using TcpService;

namespace Proxy.Connections
{
	internal class TcpConnection : ITcpConnection
	{
		public TcpConnection(ITcpServiceHandler serviceHandler) 
		{
			Connection = serviceHandler.ConnectionApi;
			Communication = serviceHandler.CommunicationApi;
		}

		public IConnectionApi Connection { get; }

		public ICommunicationApi Communication { get; }
	}
}

using Common.Connection;

namespace Proxy.MessageHandling
{
    class ProxyToMasterMessageHandler : IMessageHandler
	{
		private readonly ITcpSerializer serializer;

		private readonly ITcpSocketHandler masterSocket;

		public ProxyToMasterMessageHandler(ITcpSerializer serializer, ITcpSocketHandler socket) 
		{
			this.serializer = serializer;
			masterSocket = socket;
		}

		public void Process()
		{
			masterSocket.Send(serializer.Message);
		}
	}
}

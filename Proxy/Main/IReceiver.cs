using Proxy.Connections;

namespace Proxy
{
	internal interface IReceiver
	{
		void Receive(ITcpConnection receiveConnection, ITcpConnection sendConnection);
	}
}

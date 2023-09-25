using Proxy.Connections;
using System.Threading.Tasks;

namespace Proxy
{
	internal interface IReceiver
	{
		Task ReceiveProxy(ITcpConnection receiveConnection, ITcpConnection sendConnection);

		Task ReceiveMaster(ITcpConnection receiveConnection, ITcpConnection sendConnection);
	}
}

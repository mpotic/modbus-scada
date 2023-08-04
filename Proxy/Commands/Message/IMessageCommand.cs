using Common.Connection;
using Proxy.Connections;

namespace Proxy.Commands
{
    internal interface IMessageCommand
	{
		void SetParams(IConnection connection, ITcpSerializer serializer);

		void Execute();
	}
}

using Proxy.Connections;

namespace Proxy.Commands
{
	internal interface IMessageCommand
	{
		void SetParams(IConnection connection, byte[] message);

		void Execute();
	}
}

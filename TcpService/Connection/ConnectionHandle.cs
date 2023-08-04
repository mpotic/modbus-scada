using Common.Connection;

namespace TcpService
{
	internal sealed class ConnectionHandle : IConnectionHandle
	{
		public ITcpSocketHandler TcpSocket { get; set; } = new TcpSocketHandler();
	}
}

using Common.Connection;

namespace TcpService
{
	internal sealed class ConnectionHandle : IConnectionHandle
	{
		public ITcpSocketHandler TcpSocketHandler { get; set; }
	}
}

using Common.Connection;

namespace TcpService
{
	internal interface IConnectionHandle
	{
		ITcpSocketHandler TcpSocketHandler { get; set; }
	}
}

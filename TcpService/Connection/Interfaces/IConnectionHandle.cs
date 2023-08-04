using Common.Connection;

namespace TcpService
{
	internal interface IConnectionHandle
	{
		ITcpSocketHandler TcpSocket { get; set; }
	}
}

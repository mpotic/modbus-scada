namespace TcpService
{
	public interface IConnectionStatusCallback
	{
		void ConenctionStatusChanged(ConnectionStatusCode statusCode);
	}
}

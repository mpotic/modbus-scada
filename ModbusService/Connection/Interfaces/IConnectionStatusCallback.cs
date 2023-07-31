namespace ModbusService
{
	public interface IConnectionStatusCallback
	{
		void ConenctionStatusChanged(ConnectionStatusCode statusCode);
	}
}

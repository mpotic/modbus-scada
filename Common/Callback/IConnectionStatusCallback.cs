using Common.Enums;

namespace Common.Callback
{
	public interface IConnectionStatusCallback
	{
		void ConenctionStatusChanged(ConnectionStatusCode statusCode);
	}
}

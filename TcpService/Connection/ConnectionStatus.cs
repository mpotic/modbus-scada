using Common.Callback;
using Common.Enums;

namespace TcpService
{
	internal sealed class ConnectionStatus : IConnectionStatus
	{
		private ConnectionStatusCode statusCode = ConnectionStatusCode.Disconnected;

		public ConnectionStatusCode StatusCode
		{
			get
			{
				return statusCode;
			}
			set
			{
				if (statusCode != value)
				{
					statusCode = value;
				}

				if (Callback != null)
				{
					Callback.ConenctionStatusChanged(statusCode);
				}
			}
		}

		public IConnectionStatusCallback Callback { get; set; }
	}
}

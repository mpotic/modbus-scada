using Common.DTO;
using System.Threading.Tasks;

namespace TcpService
{
	public sealed class ConnectionApi : IConnectionApi
	{
		ISocketAccess socketAccess;

		IConnectionStatus status;

		internal ConnectionApi(ISocketAccess socketAccess, IConnectionStatus status)
		{
			this.socketAccess = socketAccess;
			this.status = status;
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			return await socketAccess.Connect(connectionParams);
		}

		public IResponse Disconnect()
		{
			return socketAccess.Disconnect();
		}

		public async Task<IResponse> Listen(IConnectionParams connectionParams)
		{
			return await socketAccess.Listen(connectionParams);
		}

		public void RegisterCallack(IConnectionStatusCallback callback)
		{
			status.Callback = callback;
		}
	}
}

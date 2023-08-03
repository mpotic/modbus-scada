using Common.Callback;
using Common.DTO;
using System.Threading.Tasks;

namespace ModbusService
{
	public sealed class ConnectionServiceApi : IConnectionServiceApi
	{
		private readonly IRtuAccess rtuAccess;

		private readonly IConnectionStatus status;

		internal ConnectionServiceApi(IRtuAccess rtuAccess, IConnectionStatus status)
		{
			this.rtuAccess = rtuAccess;
			this.status = status;
		}

		public Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			return rtuAccess.Connenct(connectionParams);
		}

		public IResponse Disconnect()
		{
			return rtuAccess.Disconnect();
		}

		public void RegisterCallack(IConnectionStatusCallback callback)
		{
			status.Callback = callback;
		}
	}
}

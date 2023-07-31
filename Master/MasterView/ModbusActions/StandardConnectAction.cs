using Common.DTO;
using MasterApi.Api;

namespace MasterView.ModbusActions
{
	class StandardConnectAction : IConnectionAction
	{
		readonly IConnectionApi connectionApi;

		private IConnectionParams connectionParams;

		public StandardConnectAction(IConnectionApi connectionApi)
		{
			this.connectionApi = connectionApi;
		}

		public void SetParams(IConnectionParams connectionParams)
		{
			this.connectionParams = connectionParams;
		}

		public void Execute()
		{
			connectionApi.StandardConnect(connectionParams);
		}
	}
}

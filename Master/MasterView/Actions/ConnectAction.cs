using Common.DTO;
using MasterApi.Api;

namespace MasterView.Actions
{
	internal sealed class ConnectAction : IConnectionAction
	{
		readonly IConnectionApi connectionApi;

		private IConnectionParams connectionParams;

		public ConnectAction(IConnectionApi connectionApi)
		{
			this.connectionApi = connectionApi;
		}

		public void SetParams(IConnectionParams connectionParams)
		{
			this.connectionParams = connectionParams;
		}

		public void Execute()
		{
			connectionApi.Connect(connectionParams);
		}
	}
}

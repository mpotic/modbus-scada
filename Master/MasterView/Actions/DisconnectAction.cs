using Common.DTO;
using MasterApi.Api;

namespace MasterView.Actions
{
	internal sealed class DisconnectAction : IConnectionAction
	{
		IConnectionApi connectionApi;

		IConnectionParams connectionParams;

		public DisconnectAction(IConnectionApi connectionApi)
		{
			this.connectionApi = connectionApi;
		}

		public void SetParams(IConnectionParams connectionParams)
		{
			this.connectionParams = connectionParams;
		}

		public void Execute()
		{
			connectionApi.Disconnect(connectionParams);
		}
	}
}

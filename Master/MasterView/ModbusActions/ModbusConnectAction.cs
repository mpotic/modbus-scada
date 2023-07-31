using Common.DTO;
using MasterApi.Api;

namespace MasterView.ModbusActions
{
	class ModbusConnectAction : IConnectionAction
	{
		readonly IConnectionApi connectionApi;

		private IConnectionParams connectionParams;

		public ModbusConnectAction(IConnectionApi connectionApi)
		{
			this.connectionApi = connectionApi;
		}

		public void SetParams(IConnectionParams connectionParams)
		{
			this.connectionParams = connectionParams;
		}

		public void Execute()
		{
			connectionApi.ModbusConnect(connectionParams);
		}

		public void SetParams(IModbusActionParams modbusParams)
		{
			connectionParams = (IConnectionParams)modbusParams;
		}
	}
}

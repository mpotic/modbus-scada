using Common.ParamsDto;
using ModbusApi.Api;

namespace ModbusView.ModbusActions
{
	class ModbusConnectAction : IModbusAction
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

		public void SetParams(IModbusParams modbusParams)
		{
			connectionParams = (IConnectionParams)modbusParams;
		}
	}
}

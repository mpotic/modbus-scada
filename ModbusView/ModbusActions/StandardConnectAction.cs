using Common.ParamsDto;
using ModbusApi.Api;

namespace ModbusView.ModbusActions
{
	class StandardConnectAction : IModbusAction
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

		public void SetParams(IModbusParams modbusParams)
		{
			connectionParams = (IConnectionParams)modbusParams;
		}
	}
}

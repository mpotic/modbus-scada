using Common.ParamsDto;
using ModbusApi.Api;

namespace ModbusView.ModbusActions
{
	class ReadCoilsAction : IModbusAction
	{
		readonly IDiscreteApi discreteApi;

		private IModbusActionParams actionParams;

		public ReadCoilsAction(IDiscreteApi discreteApi)
		{
			this.discreteApi = discreteApi;
		}

		public void Execute()
		{
			discreteApi.ReadCoil(actionParams);
		}

		public void SetParams(IModbusParams modbusParams)
		{
			actionParams = (IModbusActionParams)modbusParams;
		}
	}
}

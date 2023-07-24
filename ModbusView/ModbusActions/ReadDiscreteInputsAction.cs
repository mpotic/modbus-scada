using Common.ParamsDto;
using ModbusApi.Api;

namespace ModbusView.ModbusActions
{
	class ReadDiscreteInputsAction : IModbusAction
	{
		readonly IDiscreteApi discreteApi;

		private IModbusActionParams actionParams;

		public ReadDiscreteInputsAction(IDiscreteApi discreteApi)
		{
			this.discreteApi = discreteApi;
		}

		public void Execute()
		{
			discreteApi.ReadInput(actionParams);
		}

		public void SetParams(IModbusParams modbusParams)
		{
			actionParams = (IModbusActionParams)modbusParams;
		}
	}
}

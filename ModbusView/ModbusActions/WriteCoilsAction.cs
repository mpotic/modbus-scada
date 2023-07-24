using Common.ParamsDto;
using ModbusApi.Api;

namespace ModbusView.ModbusActions
{
	class WriteCoilsAction : IModbusAction
	{
		readonly IDiscreteApi discreteApi;

		private IModbusActionParams actionParams;

		public WriteCoilsAction(IDiscreteApi discreteApi)
		{
			this.discreteApi = discreteApi;
		}

		public void Execute()
		{
			discreteApi.WriteCoil(actionParams);
		}

		public void SetParams(IModbusParams modbusParams)
		{
			actionParams = (IModbusActionParams)modbusParams;
		}
	}
}

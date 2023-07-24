using Common.ParamsDto;
using ModbusApi.Api;

namespace ModbusView.ModbusActions
{
	class WriteHoldingAction : IModbusAction
	{
		IAnalogApi analogApi;

		IModbusActionParams actionParams;

		public WriteHoldingAction(IAnalogApi analogApi)
		{
			this.analogApi = analogApi;
		}

		public void Execute()
		{
			analogApi.WriteHolding(actionParams);
		}

		public void SetParams(IModbusParams modbusParams)
		{
			actionParams = (IModbusActionParams)modbusParams;
		}
	}
}

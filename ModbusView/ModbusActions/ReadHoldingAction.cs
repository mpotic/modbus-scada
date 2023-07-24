using Common.ParamsDto;
using ModbusApi.Api;

namespace ModbusView.ModbusActions
{
	class ReadHoldingAction : IModbusAction
	{
		IAnalogApi analogApi;

		IModbusActionParams actionParams;

		public ReadHoldingAction(IAnalogApi analogApi)
		{
			this.analogApi = analogApi;
		}

		public void Execute()
		{
			analogApi.ReadHolding(actionParams);
		}

		public void SetParams(IModbusParams modbusParams)
		{
			actionParams = (IModbusActionParams)modbusParams;
		}
	}
}

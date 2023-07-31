using Common.DTO;
using MasterApi.Api;

namespace MasterView.ModbusActions
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

		public void SetParams(IModbusActionParams modbusParams)
		{
			actionParams = modbusParams;
		}
	}
}

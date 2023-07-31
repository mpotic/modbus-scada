using Common.DTO;
using MasterApi.Api;

namespace MasterView.ModbusActions
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

		public void SetParams(IModbusActionParams modbusParams)
		{
			actionParams = modbusParams;
		}
	}
}

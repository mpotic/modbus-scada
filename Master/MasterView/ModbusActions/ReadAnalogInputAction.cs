using Common.DTO;
using MasterApi.Api;

namespace MasterView.ModbusActions
{
	public class ReadAnalogInputAction : IModbusAction
	{
		IAnalogApi analogApi;

		IModbusActionParams actionParams;

		public ReadAnalogInputAction(IAnalogApi analogApi)
		{
			this.analogApi = analogApi;
		}

		public void Execute()
		{
			analogApi.ReadInput(actionParams);
		}

		public void SetParams(IModbusActionParams modbusParams)
		{
			actionParams = modbusParams;
		}
	}
}

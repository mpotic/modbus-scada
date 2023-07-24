using Common.ParamsDto;
using ModbusApi.Api;

namespace ModbusView.ModbusActions
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

		public void SetParams(IModbusParams modbusParams)
		{
			actionParams = (IModbusActionParams)modbusParams;
		}
	}
}

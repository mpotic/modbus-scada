using Common.DTO;
using MasterApi.Api;

namespace MasterView.ModbusActions
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

		public void SetParams(IModbusActionParams modbusParams)
		{
			actionParams = modbusParams;
		}
	}
}

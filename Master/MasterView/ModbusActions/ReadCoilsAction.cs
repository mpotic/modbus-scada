using Common.DTO;
using MasterApi.Api;

namespace MasterView.ModbusActions
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

		public void SetParams(IModbusActionParams modbusParams)
		{
			actionParams = modbusParams;
		}
	}
}

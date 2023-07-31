using Common.DTO;
using MasterApi.Api;

namespace MasterView.ModbusActions
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

		public void SetParams(IModbusActionParams modbusParams)
		{
			actionParams = modbusParams;
		}
	}
}

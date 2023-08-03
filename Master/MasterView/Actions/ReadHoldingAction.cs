using Common.DTO;
using MasterApi.Api;
using MasterView.Actions;

namespace MasterView.ModbusActions
{
	class ReadHoldingAction : IReadAction
	{
		IReadApi readApi;

		IReadParams actionParams;

		public ReadHoldingAction(IReadApi readApi)
		{
			this.readApi = readApi;
		}

		public void Execute()
		{
			readApi.ReadHolding(actionParams);
		}

		public void SetParams(IReadParams readParams)
		{
			actionParams = readParams;
		}
	}
}

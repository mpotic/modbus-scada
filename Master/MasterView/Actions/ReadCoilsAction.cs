using Common.DTO;
using MasterApi.Api;
using MasterView.Actions;

namespace MasterView.ModbusActions
{
	internal sealed class ReadCoilsAction : IReadAction
	{
		readonly IReadApi readApi;

		private IReadParams actionParams;

		public ReadCoilsAction(IReadApi readApi)
		{
			this.readApi = readApi;
		}

		public void Execute()
		{
			readApi.ReadCoil(actionParams);
		}

		public void SetParams(IReadParams readParams)
		{
			actionParams = readParams;
		}
	}
}

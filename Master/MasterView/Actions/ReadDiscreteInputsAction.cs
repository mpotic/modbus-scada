using Common.DTO;
using MasterApi.Api;
using MasterView.Actions;

namespace MasterView.ModbusActions
{
	internal sealed class ReadDiscreteInputsAction : IReadAction
	{
		readonly IReadApi readApi;

		private IReadParams actionParams;

		public ReadDiscreteInputsAction(IReadApi readApi)
		{
			this.readApi = readApi;
		}

		public void Execute()
		{
			readApi.ReadDiscreteInput(actionParams);
		}

		public void SetParams(IReadParams readApi)
		{
			actionParams = readApi;
		}
	}
}

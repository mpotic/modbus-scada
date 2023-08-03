using Common.DTO;
using MasterApi.Api;

namespace MasterView.Actions
{
	internal sealed class ReadAnalogInputAction : IReadAction
	{
		IReadApi readApi;

		IReadParams actionParams;

		public ReadAnalogInputAction(IReadApi readApi)
		{
			this.readApi = readApi;
		}

		public void Execute()
		{
			readApi.ReadAnalogInput(actionParams);
		}

		public void SetParams(IReadParams readParams)
		{
			actionParams = readParams;
		}
	}
}

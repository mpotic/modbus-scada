using Common.DTO;
using MasterApi.Api;
using MasterView.Actions;

namespace MasterView.ModbusActions
{
	class WriteHoldingAction : IWriteHoldingAction
	{
		IWriteApi writeApi;

		IWriteHoldingParams actionParams;

		public WriteHoldingAction(IWriteApi writeApi)
		{
			this.writeApi = writeApi;
		}

		public void Execute()
		{
			writeApi.WriteHolding(actionParams);
		}

		public void SetParams(IWriteHoldingParams writeParams)
		{
			actionParams = writeParams;
		}
	}
}

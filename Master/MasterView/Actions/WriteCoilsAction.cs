using Common.DTO;
using MasterApi.Api;
using MasterView.Actions;

namespace MasterView.ModbusActions
{
	class WriteCoilsAction : IWriteCoilAction
	{
		IWriteApi writeApi;

		IWriteCoilParams actionParams;

		public WriteCoilsAction(IWriteApi writeApi)
		{
			this.writeApi = writeApi;
		}

		public void Execute()
		{
			writeApi.WriteCoil(actionParams);
		}

		public void SetParams(IWriteCoilParams modbusParams)
		{
			actionParams = modbusParams;
		}
	}
}

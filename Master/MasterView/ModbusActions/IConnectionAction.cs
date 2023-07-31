using Common.DTO;

namespace MasterView.ModbusActions
{
	internal interface IConnectionAction
	{
		void SetParams(IConnectionParams modbusParams);

		void Execute();
	}
}

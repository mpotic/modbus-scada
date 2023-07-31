using Common.DTO;

namespace MasterView.ModbusActions
{
	internal interface IModbusAction
	{
		void SetParams(IModbusActionParams modbusParams);

		void Execute();
	}
}

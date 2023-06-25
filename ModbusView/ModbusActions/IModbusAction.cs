using Common.ActionDto;

namespace ModbusView.ModbusActions
{
	internal interface IModbusAction
	{
		void SetParams(IModbusParams modbusParams);

		void Execute();
	}
}

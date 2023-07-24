using Common.ParamsDto;

namespace ModbusView.ModbusActions
{
	internal interface IModbusAction
	{
		void SetParams(IModbusParams modbusParams);

		void Execute();
	}
}

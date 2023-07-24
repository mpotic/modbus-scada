using Common.ParamsDto;

namespace ModbusApi.Api
{
	public interface IDiscreteApi
	{
		void ReadInput(IModbusActionParams actionParams);

		void ReadCoil(IModbusActionParams actionParams);

		void WriteCoil(IModbusActionParams actionParams);
	}
}

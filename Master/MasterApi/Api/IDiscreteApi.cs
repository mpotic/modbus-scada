using Common.DTO;

namespace MasterApi.Api
{
	public interface IDiscreteApi
	{
		void ReadInput(IModbusActionParams actionParams);

		void ReadCoil(IModbusActionParams actionParams);

		void WriteCoil(IModbusActionParams actionParams);
	}
}

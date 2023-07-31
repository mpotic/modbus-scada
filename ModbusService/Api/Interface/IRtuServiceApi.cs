using Common.DTO;
using System.Threading.Tasks;

namespace ModbusService
{
	public interface IRtuServiceApi
	{
		Task<IReadAnalogResponse> ReadHolding(IReadParams readParams);

		Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams);

		Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams);

		Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams);

		IResponse WriteHolding(IWriteHoldingParams writeParams);

		IResponse WriteCoil(IWriteCoilParams writeParams);
	}
}

using Common.DTO;
using System.Threading.Tasks;

namespace MasterServices
{
	public interface IScadaServiceProvider
	{
		Task<IResponse> Connenct(IConnectionParams connectionParams);

		IResponse Disconnect();

		Task<IReadAnalogResponse> ReadHolding(IReadParams readParams);

		Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams);

		Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams);

		Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams);

		IResponse WriteHolding(IWriteHoldingParams writeParams);

		IResponse WriteCoil(IWriteCoilParams writeParams);
	}
}

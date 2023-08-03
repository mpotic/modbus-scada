using Common.Callback;
using Common.DTO;
using System.Threading.Tasks;

namespace MasterServices
{
	public interface IRtuServiceProvider
	{
		Task<IResponse> Connect(IConnectionParams connectionParams);

		IResponse Disconnect();

		Task<IReadAnalogResponse> ReadHolding(IReadParams readParams);

		Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams);

		Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams);

		Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams);

		IResponse WriteHolding(IWriteHoldingParams writeParams);

		IResponse WriteCoil(IWriteCoilParams writeParams);

		void RegisterCallack(IConnectionStatusCallback callback);
	}
}

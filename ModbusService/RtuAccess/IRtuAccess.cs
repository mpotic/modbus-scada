using Common.DTO;
using Common.Enums;
using System.Threading.Tasks;

namespace ModbusService
{
	internal interface IRtuAccess
	{
		void TransitionState(IState state, ConnectionStatusCode statusCode);

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

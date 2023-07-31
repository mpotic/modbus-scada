using Common.DTO;
using System.Threading.Tasks;

namespace ModbusService
{
	internal class RtuAccess : IRtuAccess
	{
		IState state;

		IConnectionStatus status;

		public RtuAccess(IConnectionStatus connectionStatus)
		{
			status = connectionStatus;
		}

		public void TransitionState(IState state, ConnectionStatusCode statusCode)
		{
			this.state = state;
			status.StatusCode = statusCode;
		}

		public Task<IResponse> Connenct(IConnectionParams connectionParams)
		{
			return state.Connenct(connectionParams);
		}

		public IResponse Disconnect()
		{
			return state.Disconnect();
		}

		public async Task<IReadAnalogResponse> ReadHolding(IReadParams readParams)
		{
			return await state.ReadHolding(readParams);
		}

		public async Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams)
		{
			return await state.ReadAnalogInput(readParams);
		}

		public async Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams)
		{
			return await state.ReadCoil(readParams);
		}

		public async Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams)
		{
			return await state.ReadDiscreteInput(readParams);
		}

		public IResponse WriteHolding(IWriteHoldingParams writeParams)
		{
			return state.WriteHolding(writeParams);
		}

		public IResponse WriteCoil(IWriteCoilParams writeParams)
		{
			return state.WriteCoil(writeParams);
		}
	}
}

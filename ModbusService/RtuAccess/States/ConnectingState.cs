using Common.DTO;
using System.Threading.Tasks;

namespace ModbusService
{
	internal class ConnectingState : IState
	{
		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			return await Task.FromResult(new Response(false, "Connection attempt in progress! Please wait."));
		}

		public IResponse Disconnect()
		{
			return new Response(false, "Connection attempt in progress! Please wait.");
		}

		public async Task<IReadAnalogResponse> ReadHolding(IReadParams readParams)
		{
			return await Task.FromResult(new ReadAnalogResponse(false, "Connection attempt in progress! Please wait."));
		}

		public async Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams)
		{
			return await Task.FromResult(new ReadAnalogResponse(false, "Connection attempt in progress! Please wait."));
		}

		public async Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams)
		{
			return await Task.FromResult(new ReadDiscreteResponse(false, "Connection attempt in progress! Please wait."));
		}

		public async Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams)
		{
			return await Task.FromResult(new ReadDiscreteResponse(false, "Connection attempt in progress! Please wait."));
		}

		public IResponse WriteHolding(IWriteHoldingParams writeParams)
		{
			return new Response(false, "Connection attempt in progress! Please wait.");
		}

		public IResponse WriteCoil(IWriteCoilParams writeParams)
		{
			return new Response(false, "Connection attempt in progress! Please wait.");
		}
	}
}

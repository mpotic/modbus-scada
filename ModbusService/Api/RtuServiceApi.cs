using Common.DTO;
using System.Threading.Tasks;

namespace ModbusService
{
	public sealed class RtuServiceApi : IRtuServiceApi
	{
		private readonly IRtuAccess rtuAccess;

		internal RtuServiceApi(IRtuAccess rtuAccess)
		{
			this.rtuAccess = rtuAccess;
		}

		public async Task<IReadAnalogResponse> ReadHolding(IReadParams readParams)
		{
			return await rtuAccess.ReadHolding(readParams);
		}

		public async Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams)
		{
			return await rtuAccess.ReadAnalogInput(readParams);
		}

		public async Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams)
		{
			return await rtuAccess.ReadCoil(readParams);
		}

		public async Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams)
		{
			return await rtuAccess.ReadDiscreteInput(readParams);
		}

		public IResponse WriteHolding(IWriteHoldingParams writeParams)
		{
			return rtuAccess.WriteHolding(writeParams);	
		}

		public IResponse WriteCoil(IWriteCoilParams writeParams)
		{
			return rtuAccess.WriteCoil(writeParams);
		}
	}
}

using Common.Callback;
using Common.DTO;
using ModbusService;
using System.Threading.Tasks;

namespace MasterServices
{
	internal sealed class ModbusServiceProvider : IModbusServiceProvider
	{
		IRtuServiceApi rtuService;

		IConnectionServiceApi connectionService;

		public ModbusServiceProvider(IModbusServiceHandler modbusServiceHandler)
		{
			rtuService = modbusServiceHandler.RtuServiceApi;
			connectionService = modbusServiceHandler.ConnectionServiceApi;
		}

		public void RegisterCallback(IConnectionStatusCallback callback)
		{
			connectionService.RegisterCallack(callback);
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			IResponse response = await connectionService.Connect(connectionParams);

			return response;
		}

		public IResponse Disconnect()
		{
			IResponse response = connectionService.Disconnect();

			return response;
		}

		public async Task<IReadAnalogResponse> ReadHolding(IReadParams readParams)
		{
			IReadAnalogResponse response = await rtuService.ReadHolding(readParams);

			return response;
		}

		public async Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams)
		{
			IReadAnalogResponse response = await rtuService.ReadAnalogInput(readParams);

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams)
		{
			IReadDiscreteResponse response = await rtuService.ReadCoil(readParams);

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams)
		{
			IReadDiscreteResponse response = await rtuService.ReadDiscreteInput(readParams);

			return response;
		}

		public IResponse WriteHolding(IWriteHoldingParams writeParams)
		{
			IResponse response = rtuService.WriteHolding(writeParams);

			return response;
		}

		public IResponse WriteCoil(IWriteCoilParams writeParams)
		{
			IResponse response = rtuService.WriteCoil(writeParams);

			return response;
		}

		public void RegisterCallack(IConnectionStatusCallback callback)
		{
			connectionService.RegisterCallack(callback);
		}
	}
}

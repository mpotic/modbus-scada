using Common.DTO;
using Common.Enums;
using System;
using System.Threading.Tasks;

namespace ModbusService
{
	internal class ConnectedState : IState
	{
		private IRtuAccess rtuAccess;

		private IConnectionService connectionService;

		private IRtuService rtuService;

		private IStateFactory stateFactory;

		public ConnectedState(
			IRtuAccess rtuAccess,
			IConnectionService connectionService,
			IRtuService rtuService,
			IStateFactory stateFactory)
		{
			this.rtuAccess = rtuAccess;
			this.connectionService = connectionService;
			this.rtuService = rtuService;
			this.stateFactory = stateFactory;
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			return await Task.FromResult(new Response(false, "Already connected!"));
		}

		public IResponse Disconnect()
		{
			IResponse response;
			string customMessage = "Exception while disconnecting!\n";

			try
			{
				response = connectionService.Disconnect();
			}
			catch (Exception e)
			{
				response = new Response(false, customMessage + e.Message);
			}

			rtuAccess.TransitionState(stateFactory.GetDisconnectedState(), ConnectionStatusCode.Disconnected);

			return response;
		}

		public async Task<IReadAnalogResponse> ReadHolding(IReadParams readParams)
		{
			IReadAnalogResponse response;

			try
			{
				response = await rtuService.ReadHolding(readParams);
			}
			catch (Exception e)
			{
				response = new ReadAnalogResponse(true, "Failed to read holding registers!\n" + e.Message);

				if (!connectionService.IsConnected)
				{
					Disconnect();
				}
			}

			return response;
		}

		public async Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams)
		{
			IReadAnalogResponse response;

			try
			{
				response = await rtuService.ReadAnalogInput(readParams);
			}
			catch (Exception e)
			{
				response = new ReadAnalogResponse(true, "Failed to read analog input registers!\n" + e.Message);

				if (!connectionService.IsConnected)
				{
					Disconnect();
					rtuAccess.TransitionState(stateFactory.GetDisconnectedState(), ConnectionStatusCode.Disconnected);
				}
			}

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams)
		{
			IReadDiscreteResponse response;

			try
			{
				response = await rtuService.ReadCoil(readParams);
			}
			catch (Exception e)
			{
				response = new ReadDiscreteResponse(true, "Failed to read coil registers!\n" + e.Message);

				if (!connectionService.IsConnected)
				{
					Disconnect();
					rtuAccess.TransitionState(stateFactory.GetDisconnectedState(), ConnectionStatusCode.Disconnected);
				}
			}

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams)
		{
			IReadDiscreteResponse response;

			try
			{
				response = await rtuService.ReadDiscreteInput(readParams);
			}
			catch (Exception e)
			{
				response = new ReadDiscreteResponse(true, "Failed to read discrete input registers!\n" + e.Message);

				if (!connectionService.IsConnected)
				{
					Disconnect();
					rtuAccess.TransitionState(stateFactory.GetDisconnectedState(), ConnectionStatusCode.Disconnected);
				}
			}

			return response;
		}

		public IResponse WriteHolding(IWriteHoldingParams writeParams)
		{
			IResponse response;

			try
			{
				response = rtuService.WriteHolding(writeParams);
			}
			catch (Exception e)
			{
				response = new Response(true, "Failed to write holding registers!\n" + e.Message);

				if (!connectionService.IsConnected)
				{
					Disconnect();
					rtuAccess.TransitionState(stateFactory.GetDisconnectedState(), ConnectionStatusCode.Disconnected);
				}
			}

			return response;
		}

		public IResponse WriteCoil(IWriteCoilParams writeParams)
		{
			IResponse response;

			try
			{
				response = rtuService.WriteCoil(writeParams);
			}
			catch (Exception e)
			{
				response = new Response(true, "Failed to write coil registers!\n" + e.Message);

				if (!connectionService.IsConnected)
				{
					Disconnect();
					rtuAccess.TransitionState(stateFactory.GetDisconnectedState(), ConnectionStatusCode.Disconnected);
				}
			}

			return response;
		}
	}
}

﻿using Common.DTO;
using Common.Enums;
using System;
using System.Threading.Tasks;

namespace ModbusService
{
	internal class DisconnectedState : IState
	{
		private IRtuAccess rtuAccess;

		private IConnectionService connectionService;

		private IStateFactory stateFactory;

		public DisconnectedState(
			IRtuAccess rtuAccess,
			IConnectionService connectionService,
			IStateFactory stateFactory)
		{
			this.rtuAccess = rtuAccess;
			this.connectionService = connectionService;
			this.stateFactory = stateFactory;
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			IResponse response;
			rtuAccess.TransitionState(stateFactory.GetConnectingState(), ConnectionStatusCode.Connecting);

			try
			{
				response = await connectionService.Connect(connectionParams);
			}
			catch (Exception e)
			{
				response = new Response(false, e.Message);
				rtuAccess.TransitionState(this, ConnectionStatusCode.Disconnected);

				return response;
			}

			if (!response.IsSuccessful || !connectionService.IsConnected)
			{
				rtuAccess.TransitionState(this, ConnectionStatusCode.Disconnected);
				response = new Response(false, "Failed to connect!");

				return response;
			}
			rtuAccess.TransitionState(stateFactory.GetConnectedState(), ConnectionStatusCode.Connected);

			return response;
		}

		public IResponse Disconnect()
		{
			return new Response(false, "There is no established connection!");
		}

		public async Task<IReadAnalogResponse> ReadHolding(IReadParams readParams)
		{
			return await Task.FromResult(new ReadAnalogResponse(false, "There is no established connection!"));
		}

		public async Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams)
		{
			return await Task.FromResult(new ReadAnalogResponse(false, "There is no established connection!"));
		}

		public async Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams)
		{
			return await Task.FromResult(new ReadDiscreteResponse(false, "There is no established connection!"));
		}

		public async Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams)
		{
			return await Task.FromResult(new ReadDiscreteResponse(false, "There is no established connection!"));
		}

		public IResponse WriteHolding(IWriteHoldingParams writeParams)
		{
			return new ReadAnalogResponse(false, "There is no established connection!");
		}

		public IResponse WriteCoil(IWriteCoilParams writeParams)
		{
			return new ReadAnalogResponse(false, "There is no established connection!");
		}
	}
}

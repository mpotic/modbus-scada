using Common.DTO;
using Common.Enums;
using System;
using System.Threading.Tasks;

namespace TcpService
{
	internal sealed class DisconnectedState : IState
	{
		private readonly IConnectionService connectionService;

		private readonly ISocketAccess socketAccess;

		private readonly IStateFactory stateFactory;

		public DisconnectedState(IConnectionService connectionService, ISocketAccess socketAccess, IStateFactory stateFactory)
		{
			this.connectionService = connectionService;
			this.socketAccess = socketAccess;
			this.stateFactory = stateFactory;
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			IResponse response;
			socketAccess.TransitionState(stateFactory.GetConnectingState(), ConnectionStatusCode.Connecting);

			try
			{
				response = await connectionService.Connect(connectionParams);
			}
			catch (Exception e)
			{
				response = new Response(false, e.Message);
				socketAccess.TransitionState(this, ConnectionStatusCode.Disconnected);

				return response;
			}

			if (!response.IsSuccessful || !connectionService.IsConnected)
			{
				socketAccess.TransitionState(this, ConnectionStatusCode.Disconnected);
				response = new Response(false, "Failed to connect!");

				return response;
			}
			socketAccess.TransitionState(stateFactory.GetConnectedState(), ConnectionStatusCode.Connected);

			return response;
		}

		public IResponse Disconnect()
		{
			return new Response(false, "There is no established connection!");
		}

		public async Task<IResponse> Listen(IConnectionParams connectionParams)
		{
			IResponse response;
			socketAccess.TransitionState(stateFactory.GetConnectingState(), ConnectionStatusCode.Listening);

			try
			{
				response = await connectionService.Listen(connectionParams);
			}
			catch (Exception e)
			{
				response = new Response(false, e.Message);
				socketAccess.TransitionState(this, ConnectionStatusCode.Disconnected);
				
				return response;
			}

			if (!response.IsSuccessful || !connectionService.IsConnected)
			{
				socketAccess.TransitionState(this, ConnectionStatusCode.Disconnected);
				response = new Response(false, "Failed to connect!");

				return response;
			}
			socketAccess.TransitionState(stateFactory.GetConnectedState(), ConnectionStatusCode.Connected);

			return response;
		}

		public async Task<ITcpReceiveResponse> Receive()
		{
			return await Task.FromResult(new TcpReceiveResponse(false, "There is no established connection!"));
		}

		public IResponse Send(byte[] message)
		{
			return new Response(false, "There is no established connection!");
		}

		public async Task<ITcpReceiveResponse> ReceiveWithTimeout()
		{
			return await Task.FromResult(new TcpReceiveResponse(false, "There is no established connection!"));
		}
	}
}

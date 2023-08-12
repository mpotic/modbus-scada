using Common.DTO;
using Common.Enums;
using System;
using System.Threading.Tasks;

namespace TcpService
{
	internal sealed class ConnectedState : IState
	{
		ISocketAccess socketAccess;

		IStateFactory stateFactory;

		ICommunicationService communicationService;

		IConnectionService connectionService;

		public ConnectedState(
			ISocketAccess socketAccess,
			IStateFactory stateFactory,
			ICommunicationService communicationService,
			IConnectionService connectionService)
		{
			this.socketAccess = socketAccess;
			this.stateFactory = stateFactory;
			this.communicationService = communicationService;
			this.connectionService = connectionService;
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			return await Task.FromResult(new Response(false, "Already connected!"));
		}

		public IResponse Disconnect()
		{
			IResponse response;
			string customMessage = "Error while disconnecting!\n";

			try
			{
				response = connectionService.Disconnect();
			}
			catch (Exception e)
			{
				response = new Response(false, customMessage + e.Message);
			}

			socketAccess.TransitionState(stateFactory.GetDisconnectedState(), ConnectionStatusCode.Disconnected);

			return response;
		}

		public async Task<IResponse> Listen(IConnectionParams connectionParams)
		{
			return await Task.FromResult(new Response(false, "Already connected! Disconnect to start listening."));
		}

		public async Task<ITcpReceiveResponse> Receive()
		{
			ITcpReceiveResponse response;

			try
			{
				response = await communicationService.Receive();
			}
			catch (Exception e)
			{
				response = new TcpReceiveResponse(false, "Error while trying to receive the message!\n" + e.Message);
				if (!connectionService.IsConnected)
				{
					Disconnect();
				}
			}

			return response;
		}

		public IResponse Send(byte[] message)
		{
			IResponse response;

			try
			{
				response = communicationService.Send(message);
			}
			catch (Exception e)
			{
				response = new TcpReceiveResponse(false, "Error while trying to send the message!\n" + e.Message);
				if (!connectionService.IsConnected)
				{
					Disconnect();
				}
			}

			return response;
		}

		public async Task<ITcpReceiveResponse> ReceiveWithTimeout()
		{
			ITcpReceiveResponse response;

			try
			{
				response = await communicationService.ReceiveWithTimeout();
			}
			catch (Exception e)
			{
				response = new TcpReceiveResponse(false, "Error while trying to receive the message!\n" + e.Message);
				if (!connectionService.IsConnected)
				{
					Disconnect();
				}
			}

			return response;
		}
	}
}

using Common.DTO;
using Common.Enums;
using System;
using System.Threading.Tasks;

namespace TcpService
{
	internal sealed class ListeningState : IState
	{
		private readonly IConnectionService connectionService;

		private readonly ISocketAccess socketAccess;

		private readonly IStateFactory stateFactory;

		public ListeningState(IConnectionService connectionService, ISocketAccess socketAccess, IStateFactory stateFactory)
		{
			this.connectionService = connectionService;
			this.socketAccess = socketAccess;
			this.stateFactory = stateFactory;
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			return await Task.FromResult(new Response(false, "Listening attempt in progress! Disconnect to stop listening."));
		}

		public IResponse Disconnect()
		{
			IResponse response;

			try
			{
				response = connectionService.Disconnect();
			}
			catch (Exception e)
			{
				response = new Response(false, "Exception while stopping listening!\n" + e.Message);
			}

			socketAccess.TransitionState(stateFactory.GetDisconnectedState(), ConnectionStatusCode.Disconnected);

			return response;
		}

		public async Task<IResponse> Listen(IConnectionParams connectionParams)
		{
			return await Task.FromResult(new Response(false, "Listening attempt already in progress!"));
		}

		public async Task<ITcpReceiveResponse> Receive()
		{
			return await Task.FromResult(new TcpReceiveResponse(false, "Listening attempt in progress! Disconnect to stop listening."));
		}

		public IResponse Send(byte[] message)
		{
			return new Response(false, "Listening attempt in progress! Disconnect to stop listening.");
		}

		public async Task<ITcpReceiveResponse> ReceiveWithTimeout()
		{
			return await Task.FromResult(new TcpReceiveResponse(false, "Listening attempt in progress! Disconnect to stop listening."));
		}
	}
}

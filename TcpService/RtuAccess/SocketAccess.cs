using Common.DTO;
using Common.Enums;
using System.Threading.Tasks;

namespace TcpService
{
	internal class SocketAccess : ISocketAccess
	{
		IState state;

		IConnectionStatus status;

		internal SocketAccess(IConnectionStatus status)
		{
			this.status = status;
		}

		public void TransitionState(IState state, ConnectionStatusCode statusCode)
		{
			this.state = state;
			status.StatusCode = statusCode;
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			return await state.Connect(connectionParams);
		}

		public IResponse Disconnect()
		{
			return state.Disconnect();
		}

		public async Task<IResponse> Listen(IConnectionParams connectionParams)
		{
			return await state.Listen(connectionParams);
		}

		public async Task<ITcpReceiveResponse> Receive()
		{
			return await state.Receive();
		}

		public IResponse Send(byte[] message)
		{
			return state.Send(message);
		}
	}
}

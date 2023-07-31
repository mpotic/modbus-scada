using Common.DTO;
using System.Threading.Tasks;

namespace TcpService
{
	internal sealed class ConnectingState : IState
	{
		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			return await Task.FromResult(new Response(false, "Connection attempt in progress! Please wait."));
		}

		public IResponse Disconnect()
		{
			return new Response(false, "Connection attempt in progress! Please wait.");
		}

		public async Task<IResponse> Listen(IConnectionParams connectionParams)
		{
			return await Task.FromResult(new Response(false, "Connection attempt in progress! Please wait."));
		}

		public async Task<ITcpReceiveResponse> Receive()
		{
			return await Task.FromResult(new TcpReceiveResponse(false, "Connection attempt in progress! Please wait."));
		}

		public IResponse Send(byte[] message)
		{
			return new Response(false, "Connection attempt in progress! Please wait.");
		}
	}
}

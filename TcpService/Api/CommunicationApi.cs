using Common.DTO;
using System.Threading.Tasks;

namespace TcpService
{
	public sealed class CommunicationApi : ICommunicationApi
	{
		ISocketAccess socketAccess;

		internal CommunicationApi(ISocketAccess socketAccess)
		{
			this.socketAccess = socketAccess;
		}

		public async Task<ITcpReceiveResponse> Receive()
		{
			return await socketAccess.Receive();
		}

		public async Task<ITcpReceiveResponse> ReceiveWithTimeout()
		{
			return await socketAccess.ReceiveWithTimeout();
		}

		public IResponse Send(byte[] message)
		{
			return socketAccess.Send(message);
		}
	}
}

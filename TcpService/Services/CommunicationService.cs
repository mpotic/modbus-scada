using Common.DTO;
using System.Threading.Tasks;

namespace TcpService
{
	internal class CommunicationService : ICommunicationService
	{
		private readonly IConnectionHandle connectionHandle;

		public CommunicationService(IConnectionHandle connectionHandle)
		{
			this.connectionHandle = connectionHandle;
		}

		public async Task<ITcpReceiveResponse> Receive()
		{
			byte[] values = await connectionHandle.TcpSocketHandler.ReceiveAsync();
			ITcpReceiveResponse response = new TcpReceiveResponse(true, values);

			return response;
		}

		public IResponse Send(byte[] message)
		{
			connectionHandle.TcpSocketHandler.Send(message);

			return new Response(true);
		}
	}
}

using Common.DTO;
using System;
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
			try
			{
				byte[] values = await connectionHandle.TcpSocket.ReceiveAsync();
				return new TcpReceiveResponse(true, values);
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public IResponse Send(byte[] message)
		{
			connectionHandle.TcpSocket.Send(message);

			return new Response(true);
		}

		public async Task<ITcpReceiveResponse> ReceiveWithTimeout()
		{
			try
			{
				byte[] values = await connectionHandle.TcpSocket.ReceiveWithTimeout();
				return new TcpReceiveResponse(true, values);
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}

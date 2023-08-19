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
			ITcpReceiveResponse response;
			byte[] values;
			try
			{
				values = await connectionHandle.TcpSocket.ReceiveAsync();
			}
			catch (Exception e)
			{
				throw e;
			}

			if (values.Length == 0)
			{
				response = new TcpReceiveResponse(false, "No data was received!");
			}
			else
			{
				response = new TcpReceiveResponse(true, values);
			}

			return response;
		}

		public async Task<ITcpReceiveResponse> ReceiveWithTimeout()
		{
			ITcpReceiveResponse response;
			byte[] values;
			try
			{
				values = await connectionHandle.TcpSocket.ReceiveWithTimeout();
			}
			catch (Exception e)
			{
				throw e;
			}

			if (values.Length == 0)
			{
				response = new TcpReceiveResponse(false, "Timeout exceeded, no data was received!");
			}
			else
			{
				response = new TcpReceiveResponse(true, values);
			}

			return response;
		}

		public IResponse Send(byte[] message)
		{
			connectionHandle.TcpSocket.Send(message);

			return new Response(true);
		}

		public IResponse ClearReceiveBuffer()
		{
			connectionHandle.TcpSocket.ClearReceiveBuffer();

			return new Response(true);
		}
	}
}

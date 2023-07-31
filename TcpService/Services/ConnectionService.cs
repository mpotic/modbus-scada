using Common.DTO;
using System.Threading.Tasks;

namespace TcpService
{
	internal sealed class ConnectionService : IConnectionService
	{
		private readonly IConnectionHandle connectionHandle;

		public ConnectionService(IConnectionHandle connectionHandle)
		{
			this.connectionHandle = connectionHandle;
		}

		public bool IsConnected
		{
			get
			{
				return connectionHandle.TcpSocketHandler.IsConnected;
			}
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			await connectionHandle.TcpSocketHandler.ConnectAsync(connectionParams.RemotePort, connectionParams.LocalPort);

			return new Response(true);
		}

		public IResponse Disconnect()
		{
			connectionHandle.TcpSocketHandler.CloseAndUnbindWorkingSocket();
			connectionHandle.TcpSocketHandler.CloseListening();

			return new Response(true);
		}

		public async Task<IResponse> Listen(IConnectionParams connectionParams)
		{
			connectionHandle.TcpSocketHandler.Listen(connectionParams.LocalPort);
			await connectionHandle.TcpSocketHandler.AcceptAsync();
			connectionHandle.TcpSocketHandler.CloseListening();

			return new Response(true);
		}
	}
}

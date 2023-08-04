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
				return connectionHandle.TcpSocket.IsConnected;
			}
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			await connectionHandle.TcpSocket.ConnectAsync(connectionParams.RemotePort, connectionParams.LocalPort);

			return new Response(true);
		}

		public IResponse Disconnect()
		{
			connectionHandle.TcpSocket.CloseAndUnbindWorkingSocket();
			connectionHandle.TcpSocket.CloseListening();

			return new Response(true);
		}

		public async Task<IResponse> Listen(IConnectionParams connectionParams)
		{
			connectionHandle.TcpSocket.Listen(connectionParams.LocalPort);
			await connectionHandle.TcpSocket.AcceptAsync();
			connectionHandle.TcpSocket.CloseListening();

			return new Response(true);
		} 
	}
}

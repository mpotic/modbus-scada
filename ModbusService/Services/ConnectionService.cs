using NModbus;
using System.Net.Sockets;
using System.Net;
using Common.DTO;
using System.Threading.Tasks;

namespace ModbusService
{
	internal class ConnectionService : IConnectionService
	{
		private readonly IConnectionHandle connection;

		public ConnectionService(IConnectionHandle handle)
		{
			connection = handle;
		}

		public bool IsConnected
		{
			get
			{
				return connection.TcpClient != null && connection.TcpClient.Connected && connection.ModbusMaster != null;
			}
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Loopback, connectionParams.LocalPort);
			connection.TcpClient = new TcpClient(localEndpoint);
			connection.TcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			await connection.TcpClient.Client.ConnectAsync(IPAddress.Loopback, connectionParams.RemotePort);
			IModbusFactory factory = new ModbusFactory();
			connection.ModbusMaster = factory.CreateMaster(connection.TcpClient);

			if (connection.ModbusMaster == null || !connection.TcpClient.Client.Connected)
			{
				Disconnect();

				return new Response(false, "Connection attempt failed!");
			}

			return new Response(true);
		}

		public IResponse Disconnect()
		{
			if (connection.TcpClient != null)
			{
				connection.TcpClient.Close();
			}
			connection.TcpClient = null;
			connection.ModbusMaster.Dispose();
			connection.ModbusMaster = null;

			return new Response(true);
		}
	}
}

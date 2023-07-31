using Common.Connection;
using MasterServices.Connection;
using NModbus;
using System;
using System.Net;
using System.Net.Sockets;
using Common.Enums;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;

namespace MasterServices.Services
{
	internal class ConnectionService : IConnectionService
	{
		private ConnectedServiceStatus serviceStatus = ConnectedServiceStatus.None;

		private IServiceHandler serviceHandler;

		public ConnectionService(IServiceHandler serviceHandler)
		{
			this.serviceHandler = serviceHandler;
		}

		public IModbusConnection ModbusConnection { get; } = new ModbusConnection();

		public IStandardConnection StandardConnection { get; } = new StandardConnection();

		public IResponse ModbusConnect(IConnectionParams connectionParams)
		{
			SetupModbusServices();

			try
			{
				TcpClient client = GetTcpClient(connectionParams);
				IModbusFactory factory = new ModbusFactory();
				IModbusMaster connection = factory.CreateMaster(client);

				ModbusConnection.ModbusMaster = connection;
			}
			catch (Exception e)
			{
				DisconnectModbus();
				ModbusConnection.ModbusMaster = null;

				return new Response(false, e.Message);
			}

			return new Response(true);
		}

		public async Task<IResponse> StandardConnect(IConnectionParams connectionParams)
		{
			SetupStandardServices();

			try
			{
				StandardConnection.Connection = new TcpSocketHandler();
				await StandardConnection.Connection.ConnectAsync(connectionParams.RemotePort, connectionParams.LocalPort);
			}
			catch (Exception e)
			{
				DisconnectStandard();
				StandardConnection.Connection = null;

				return new Response(false, e.Message);
			}

			return new Response(true);
		}

		/// <summary>
		/// Instantiate modbus services if they are not already instantiated. Terminates any existing connection.
		/// </summary>
		private void SetupModbusServices()
		{
			Disconnect();
			if (serviceStatus != ConnectedServiceStatus.Modbus)
			{
				((ServiceHandler)serviceHandler).SetupModbusServices();
				serviceStatus = ConnectedServiceStatus.Modbus;
			}
		}

		/// <summary>
		/// Instantiate standard services if they are not already instantiated. Terminates any existing connection.
		/// </summary>
		private void SetupStandardServices()
		{
			Disconnect();
			if (serviceStatus != ConnectedServiceStatus.Standard)
			{
				((ServiceHandler)serviceHandler).SetupStandardServices();
				serviceStatus = ConnectedServiceStatus.Standard;
			}
		}

		private TcpClient GetTcpClient(IConnectionParams connectionParams)
		{
			IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Loopback, connectionParams.LocalPort);
			TcpClient tcpClient = new TcpClient(localEndpoint);
			ModbusConnection.ClientConnection = tcpClient;
			tcpClient.Client.Connect(IPAddress.Loopback, connectionParams.RemotePort);

			return tcpClient;
		}

		public IResponse Disconnect()
		{
			IResponse response;
			if (ModbusConnection.ClientConnection != null)
			{
				response = DisconnectModbus();
			}
			else if(StandardConnection.Connection != null) 
			{
				response = DisconnectStandard();
			}
			else
			{
				response = new Response(false, "There is no established connection!");
			}

			return response;
		}

		private IResponse DisconnectModbus()
		{
			try
			{
				if (ModbusConnection.ClientConnection != null)
				{
					ModbusConnection.ClientConnection.Close();
				}
				ModbusConnection.ClientConnection = null;
            }
			catch (Exception ex)
			{
				return new Response(false, ex.Message);
			}

			return new Response(true);
		}

		private IResponse DisconnectStandard()
		{
			try
			{
				if (StandardConnection.Connection != null && StandardConnection.Connection.IsConnected)
				{
					byte[] closeMessage = Encoding.UTF8.GetBytes(SenderCode.CloseConnection.ToString());
					StandardConnection.Connection.Send(closeMessage);
				}
				StandardConnection.Connection.CloseAndUnbindWorkingSocket();
				StandardConnection.Connection = null;
			}
			catch (Exception ex)
			{
				return new Response(false, ex.Message);
			}

			return new Response(true);
		}
	}
}

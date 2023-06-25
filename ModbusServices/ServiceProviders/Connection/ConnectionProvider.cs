using Common.ActionDto;
using Common.ResponseDto;
using ModbusServices.Connection;
using ModbusServices.Services;
using NModbus;
using System;
using System.Net;
using System.Net.Sockets;

namespace ModbusServices.ServiceProviders
{
	public class ConnectionProvider : IConnectionProvider
	{
		private ConnectedServiceStatus serviceStatus = ConnectedServiceStatus.None;

		private IServiceHandler serviceHandler;

		public ConnectionProvider(IServiceHandler serviceHandler)
		{
			this.serviceHandler = serviceHandler;
		}

		internal IModbusConnection ModbusConnection { get; } = new ModbusConnection();

		internal IStandardConnection StandardConnection { get; } = new StandardConnection();

		/// <summary>
		/// Initialize the modbus services if needed. Initialize the connection to the modbus slave using IModbusMaster.
		/// </summary>
		/// <param name="connectionParams">Specifies clients and servers port.</param>
		/// <returns></returns>
		public IOperationResponse ModbusConnect(IConnectionParams connectionParams)
		{
			SetupModbusServices();

			try
			{
				TcpClient client = GetTcpClientConnection(connectionParams.ServerPort, connectionParams.ClientPort);
				
				IModbusFactory factory = new ModbusFactory();
				IModbusMaster connection = factory.CreateMaster(client);

				ModbusConnection.ModbusMaster = connection;
			}
			catch (Exception e)
			{
				return new OperationResponse(false, e.Message);
			}

			return new OperationResponse(true);
		}

		public IOperationResponse StandardConnect(IConnectionParams connectionParams)
		{
			SetupStandardServices();

			try
			{
				StandardConnection.Connection.Connect(connectionParams.ServerPort, connectionParams.ClientPort);
			}
			catch(Exception e)
			{
				return new OperationResponse(false, e.Message);
			}

			return new OperationResponse(true);
		}

		/// <summary>
		/// Instantiate modbus services if they are not already instantiated.
		/// </summary>
		public void SetupModbusServices()
		{
			if (serviceStatus != ConnectedServiceStatus.Modbus)
			{
				((ServiceHandler)serviceHandler).SetupModbusServices();
				serviceStatus = ConnectedServiceStatus.Modbus;
			}
		}

		/// <summary>
		/// Instantiate standard services if they are not already instantiated.
		/// </summary>
		public void SetupStandardServices()
		{
			if (serviceStatus != ConnectedServiceStatus.Standard)
			{
				((ServiceHandler)serviceHandler).SetupStandardServices();
				serviceStatus = ConnectedServiceStatus.Standard;
			}
		}

		private TcpClient GetTcpClientConnection(int serverPort, int clientPort)
		{
			TcpClient tcpClient = new TcpClient();

			tcpClient.Client.Bind(new IPEndPoint(IPAddress.Loopback, serverPort));
			tcpClient.Client.Connect(IPAddress.Loopback, clientPort);
			
			return tcpClient;
		}
	}
}

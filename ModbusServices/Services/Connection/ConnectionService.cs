using Common.ActionDto;
using Common.Connection;
using Common.ResponseDto;
using ModbusServices.Connection;
using NModbus;
using System;
using System.Net;
using System.Net.Sockets;

namespace ModbusServices.Services
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

		public IOperationResponse ModbusConnect(IConnectionParams connectionParams)
		{
			SetupModbusServices();

			try
			{
				TcpClient client = GetTcpClient(connectionParams.ClientPort);
				
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
				StandardConnection.Connection = new TcpSocketHandler();
				StandardConnection.Connection.Connect(connectionParams.ClientPort);
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
		private void SetupModbusServices()
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
		private void SetupStandardServices()
		{
			if (serviceStatus != ConnectedServiceStatus.Standard)
			{
				((ServiceHandler)serviceHandler).SetupStandardServices();
				serviceStatus = ConnectedServiceStatus.Standard;
			}
		}

		private TcpClient GetTcpClient(int clientPort)
		{
			TcpClient tcpClient = new TcpClient();

			tcpClient.Client.Connect(IPAddress.Loopback, clientPort);
			
			return tcpClient;
		}
	}
}

using Common.DTO;
using ModbusService;
using Proxy.Connections;
using Proxy.Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TcpService;

namespace Proxy
{
	internal class ProxyWorker : IProxyWorker
	{
		IReceiver receiver;

		ISecurityHandler security;

		IDictionary<int, ITcpConnection> connections = new Dictionary<int, ITcpConnection>();

		Tuple<int, IModbusConnection> modbusConnection;

		public ProxyWorker(ISecurityHandler security)
		{
			this.security = security;
			modbusConnection = new Tuple<int, IModbusConnection>(0, new ModbusConnection(new ModbusServiceHandler()));
			receiver = new Receiver(modbusConnection.Item2, security);
		}

		public async void Connect(string protocol, int localPort, int remotePort)
		{
			try
			{
				if (protocol == "modbus")
				{
					await ConnectModbus(localPort, remotePort);
				}
				else if (protocol == "tcp")
				{
					await ConnectTcp(localPort, remotePort);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private async Task ConnectTcp(int localPort, int remotePort)
		{
			if (connections.ContainsKey(localPort))
			{
				throw new Exception("Port already in use!");
			}

			IConnectionParams connectionParams = new ConnectionParams(remotePort, localPort);
			ITcpServiceHandler serviceHandler = new TcpServiceHandler();
			IResponse response = await serviceHandler.ConnectionApi.Connect(connectionParams);
			if (!response.IsSuccessful)
			{
				throw new Exception("Failed to connect to proxy! " + response.ErrorMessage);
			}

			ITcpConnection connection = new TcpConnection(serviceHandler);
			connections.Add(localPort, connection);
			Console.WriteLine($"Tcp connected from {localPort} to {remotePort} successfully.");
		}

		private async Task ConnectModbus(int localPort, int remotePort)
		{
			if (connections.ContainsKey(localPort))
			{
				throw new Exception("Port already in use!");
			}

			IConnectionParams connectionParams = new ConnectionParams(remotePort, localPort);
			IResponse response = await modbusConnection.Item2.Connection.Connect(connectionParams);
			if (!response.IsSuccessful)
			{
				throw new Exception("Failed to connect to slave! " + response.ErrorMessage);
			}

			modbusConnection = new Tuple<int, IModbusConnection>(localPort, modbusConnection.Item2);
			Console.WriteLine($"Modbus connected from {localPort} to {remotePort} successfully.");
		}

		public void Disconnect(int port)
		{
			if (modbusConnection.Item1 == port)
			{
				modbusConnection.Item2.Connection.Disconnect();
				Console.WriteLine($"Disconnected Modbus on port {port}.");
				modbusConnection = new Tuple<int, IModbusConnection>(0, modbusConnection.Item2);

				return;
			}

			if (!connections.ContainsKey(port))
			{
				throw new Exception("Port can not be disconnected because its not in use!");
			}

			connections[port].Connection.Disconnect();
			Console.WriteLine($"Disconnected TCP on port {port}.");
			connections.Remove(port);
		}

		public async void Listen(int port)
		{
			try
			{
				if (connections.ContainsKey(port))
				{
					throw new Exception("Port already in use!");
				}

				IConnectionParams connectionParams = new ConnectionParams(port);
				ITcpServiceHandler serviceHandler = new TcpServiceHandler();
				Console.WriteLine($"Listening on {port} port.");

				IResponse response = await serviceHandler.ConnectionApi.Listen(connectionParams);
				if (!response.IsSuccessful)
				{
					throw new Exception("Listening failed! " + response.ErrorMessage);
				}
				ITcpConnection connection = new TcpConnection(serviceHandler);
				connections.Add(port, connection);
				Console.WriteLine($"Accepted connection on listening port {port}.");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		public void Receive(int receivePort, int sendPort)
		{
			ITcpConnection receiveConnection;
			connections.TryGetValue(receivePort, out receiveConnection);
			ITcpConnection sendConnection;
			connections.TryGetValue(sendPort, out sendConnection);

			if (receiveConnection == null)
			{
				throw new Exception($"Port {receivePort} not connected!");
			}

			if (sendConnection == null)
			{
				throw new Exception($"Port {sendPort} not connected!");
			}

			receiver.Receive(receiveConnection, sendConnection);

			Console.WriteLine($"Receiving on {receivePort} port, forwarding to {sendPort} port.");
		}

		public void ListAllConections()
		{
			string list = "";
			foreach (int port in connections.Keys)
			{
				list += $"Tcp Port: {port}\n";
			}

			list += modbusConnection.Item1 != 0 ? $"Modbus Port: {modbusConnection.Item1}\n" : "";
			Console.Write(list);
		}
	}
}

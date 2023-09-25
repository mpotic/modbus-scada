using Common.DTO;
using ModbusService;
using Proxy.Connections;
using Proxy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TcpService;

namespace Proxy
{
	internal class ProxyWorker : IProxyWorker
	{
		IReceiver receiver;

		IDictionary<int, ITcpConnection> connections = new Dictionary<int, ITcpConnection>();

		Tuple<int, IModbusConnection> modbusConnection;

		List<Tuple<int, int>> receiveSendPorts = new List<Tuple<int, int>>();

		public ProxyWorker(ISecurityHandler security)
		{
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

		public void PrepareReceive(int recPort, int sendPort, out ITcpConnection recCon, out ITcpConnection sendCon)
		{
			if(receiveSendPorts.Contains(new Tuple<int, int>(recPort, sendPort)))
			{
                throw new Exception($"Already receiving on {recPort} and sending to {sendPort}.");
            }

			connections.TryGetValue(recPort, out recCon);
			connections.TryGetValue(sendPort, out sendCon);

			if (recCon == null)
			{
				throw new Exception($"Port {sendPort} not connected!");
			}

			if (sendCon == null)
			{
				throw new Exception($"Port {sendPort} not connected!");
			}
		}

		public async void ReceiveProxy(int receivePort, int sendPort)
		{
			var ports = new Tuple<int, int>(receivePort, sendPort);

			PrepareReceive(receivePort, sendPort, out ITcpConnection recCon, out ITcpConnection sendCon);
			receiveSendPorts.Add(new Tuple<int, int>(receivePort, sendPort));
			Console.WriteLine($"Receiving on {receivePort} port, forwarding to {sendPort} port.");

			await receiver.ReceiveProxy(recCon, sendCon);
			receiveSendPorts.Remove(ports);
		}

		public async void ReceiveMaster(int receivePort, int sendPort)
		{
			var ports = new Tuple<int, int>(receivePort, sendPort);

			PrepareReceive(receivePort, sendPort, out ITcpConnection recCon, out ITcpConnection sendCon);
			receiveSendPorts.Add(ports);
			Console.WriteLine($"Receiving on {receivePort} port, forwarding to {sendPort} port.");

			await receiver.ReceiveMaster(recCon, sendCon);
			receiveSendPorts.Remove(ports);
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

using Common.Connection;
using Common.DTO;
using Common.Enums;
using Proxy.Commands;
using Proxy.Connections;
using Proxy.Security;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Proxy
{
	internal class Receiver : IReceiver
	{
		IDictionary<SenderCode, IMessageCommand> processingCommand;

		ISecurityHandler security;

		ITcpSerializer serializer = new TcpSerializer();

		public Receiver(IModbusConnection modbusConnection, ISecurityHandler security)
		{
			processingCommand = new Dictionary<SenderCode, IMessageCommand>
			{
				{ SenderCode.Master, new MasterMessageCommand(security) },
				{ SenderCode.ProxyToMaster, new ProxyToMasterMessageCommand() },
				{ SenderCode.ProxyToSlave, new ProxyToSlaveMessageCommand(modbusConnection, security) }
			};

			this.security = security;
		}

		public async Task ReceiveProxy(ITcpConnection receiveConnection, ITcpConnection sendConnection)
		{
			while (true)
			{
				try
				{
					byte[] message = await ReceiveSingleMessage(receiveConnection);
					if (message == null || message.Length == 0)
					{
						break;
					}

					ProcessProxyMessage(message, sendConnection);
				}
				catch (SocketException ex)
				when (ex.SocketErrorCode == SocketError.OperationAborted || ex.SocketErrorCode == SocketError.Shutdown)
				{
					Console.WriteLine("Receiving socket closed.");
					break;
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}

		public async Task ReceiveMaster(ITcpConnection receiveConnection, ITcpConnection sendConnection)
		{
			while (true)
			{
				try
				{
					byte[] message = await ReceiveSingleMessage(receiveConnection);
					if (message == null || message.Length == 0)
					{
						break;
					}

					ProcessMasterMessage(message, sendConnection);
				}
				catch (SocketException ex)
				when (ex.SocketErrorCode == SocketError.OperationAborted || ex.SocketErrorCode == SocketError.Shutdown)
				{
					Console.WriteLine("Receiving socket closed.");
					break;
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}

		private async Task<byte[]> ReceiveSingleMessage(ITcpConnection receiveConnection)
		{
			ITcpReceiveResponse response = await receiveConnection.Communication.Receive();

			if (response.Payload == null || response.Payload.Length <= 0)
			{
				Console.WriteLine("Receiving aborted.");

				return null;
			}

			if (!response.IsSuccessful)
			{
				throw new Exception(response.ErrorMessage);
			}

			return response.Payload;
		}

		private void ProcessMasterMessage(byte[] message, ITcpConnection sendConnection)
		{
			Console.WriteLine($"- - - - - - - - - - Master message - - - - - - - - - -");

			serializer.InitMessage(message);
			SenderCode senderCode = serializer.ReadSenderCodeFromHeader();
			Console.WriteLine("Serialized: " + serializer);

			processingCommand[senderCode].SetParams(sendConnection, message);
			processingCommand[senderCode].Execute();
		}

		private void ProcessProxyMessage(byte[] message, ITcpConnection sendConnection)
		{
			Console.WriteLine($"- - - - - - - - - - Proxy message - - - - - - - - - -");

			byte[] extractedData = HandleSecurity(message);
			serializer.InitMessage(extractedData);
			SenderCode senderCode = serializer.ReadSenderCodeFromHeader();
			Console.WriteLine("Serialized: " + serializer);

			processingCommand[senderCode].SetParams(sendConnection, extractedData);
			processingCommand[senderCode].Execute();
		}

		public byte[] HandleSecurity(byte[] input)
		{
			byte[] validatedInput = security.Validate(input);

			return validatedInput;
		}
	}
}

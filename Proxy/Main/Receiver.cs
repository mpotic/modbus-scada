using Common.Connection;
using Common.DTO;
using Common.Enums;
using Proxy.Commands;
using Proxy.Connections;
using Proxy.Security;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

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
				{ SenderCode.ProxyToMaster, new ProxyToMasterMessageCommand(security) },
				{ SenderCode.ProxyToSlave, new ProxyToSlaveMessageCommand(modbusConnection, security) }
			};

			this.security = security;
		}

		public async void Receive(ITcpConnection receiveConnection, ITcpConnection sendConnection)
		{
			while (true)
			{
				try
				{
					ITcpReceiveResponse response = await receiveConnection.Communication.Receive();

					if (response.Payload == null || response.Payload.Length <= 0)
					{
						Console.WriteLine("Receiving aborted.");

						return;
					}

					if (!response.IsSuccessful)
					{
						throw new Exception(response.ErrorMessage);
					}

					Console.WriteLine($"- - - - - - - - - - New message - - - - - - - - - -");

					byte[] message = HandleSecurity(response.Payload);
					serializer.InitMessage(message);
					SenderCode senderCode = serializer.ReadSenderCodeFromHeader();

					Console.WriteLine("Payload: " + @Encoding.UTF8.GetString(response.Payload));
					Console.WriteLine("Serialized: " + serializer);

					processingCommand[senderCode].SetParams(sendConnection, message);
					processingCommand[senderCode].Execute();
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

		public byte[] HandleSecurity(byte[] input)
		{
			if (serializer.IsByteArrayTcpSerializedData(input))
			{
				return input;
			}

			byte[] validatedInput = security.Validate(input);

			return validatedInput;
		}
	}
}

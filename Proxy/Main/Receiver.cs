using Common.Connection;
using Common.DTO;
using Common.Enums;
using Proxy.Commands;
using Proxy.Connections;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Proxy
{
	internal class Receiver : IReceiver
	{
		IDictionary<SenderCode, IMessageCommand> processingCommand;

		public Receiver(IModbusConnection modbusConnection)
		{
			processingCommand = new Dictionary<SenderCode, IMessageCommand>
			{
				{ SenderCode.Master, new MasterMessageCommand() },
				{ SenderCode.ProxyToMaster, new ProxyToMasterMessageCommand() },
				{ SenderCode.ProxyToSlave, new ProxyToSlaveMessageCommand(modbusConnection) }
			};
		}

		public async void Receive(ITcpConnection receiveConnection, ITcpConnection sendConnection)
		{
			ITcpSerializer messageSerializer = new TcpSerializer();

			while (true)
			{
				try
				{
					ITcpReceiveResponse response = await receiveConnection.Communication.Receive();

					if(response.Payload == null || response.Payload.Length <= 0)
					{
						Console.WriteLine("Receiving aborted.");

                        return;
					}

					if (!response.IsSuccessful)
					{
						throw new Exception(response.ErrorMessage);
					}

					messageSerializer.InitMessage(response.Payload);
                    Console.WriteLine(messageSerializer);
                    SenderCode senderCode = messageSerializer.ReadSenderCodeFromHeader();
					processingCommand[senderCode].SetParams(sendConnection, messageSerializer);
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
	}
}

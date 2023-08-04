using Common.Connection;
using Common.DTO;
using Common.Enums;
using Proxy.Commands;
using Proxy.Connections;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Proxy
{
	internal class Receiver : IReceiver
	{
		IDictionary<SenderCode, IMessageCommand> commands;

		public Receiver(IModbusConnection modbusConnection)
		{
			commands = new Dictionary<SenderCode, IMessageCommand>
			{
				{ SenderCode.Master, new MasterMessageCommand() },
				{ SenderCode.ProxyToMaster, new ProxyToMasterMessageCommand() },
				{ SenderCode.ProxyToSlave, new ProxyToSlaveMessageCommand(modbusConnection) }
			};
		}

		public async void Receive(ITcpConnection receiveConnection, ITcpConnection sendConnection)
		{
			ITcpSerializer message = new TcpSerializer();

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

					PrintReceivedMessage(response.Payload);
					message.InitMessage(response.Payload);
					SenderCode senderCode = message.ReadSenderCodeFromHeader();
					commands[senderCode].SetParams(sendConnection, message);
					commands[senderCode].Execute();
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

		private void PrintReceivedMessage(byte[] message)
		{
			Console.WriteLine(Encoding.UTF8.GetString(message));
		}
	}
}

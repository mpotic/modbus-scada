using Common.Connection;
using Common.Enums;
using Proxy.Connections;
using Proxy.Security;
using System;
using System.Collections.Generic;

namespace Proxy.Commands
{
	sealed class ProxyToSlaveMessageCommand : IMessageCommand
	{
		private readonly Dictionary<FunctionCode, IModbusReadCommand> modbusReadFunctions;

		private readonly Dictionary<FunctionCode, IModbusWriteCommand> modbusWriteFunction;

		private ITcpConnection proxy;

		private byte[] message;

		private readonly ISecurityHandler security;

		public ProxyToSlaveMessageCommand(IModbusConnection slave, ISecurityHandler security)
		{
			modbusReadFunctions = new Dictionary<FunctionCode, IModbusReadCommand>
			{
				{ FunctionCode.ReadCoils, new ReadCoilCommand(slave) },
				{ FunctionCode.ReadHolding, new ReadHoldingCommand(slave) },
				{ FunctionCode.ReadDiscreteInputs, new ReadDiscreteInputCommand(slave) },
				{ FunctionCode.ReadAnalogInputs, new ReadAnalogInputCommand(slave) },
			};

			modbusWriteFunction = new Dictionary<FunctionCode, IModbusWriteCommand>
			{
				{ FunctionCode.WriteCoils, new WriteCoilCommand(slave) },
				{ FunctionCode.WriteHolding, new WriteHoldingCommand(slave) }
			};

			this.security = security;
		}

		public void SetParams(IConnection connection, byte[] message)
		{
			proxy = (ITcpConnection)connection;
			this.message = message;
		}

		public void Execute()
		{
			ITcpSerializer serializer = new TcpSerializer();
			serializer.InitMessage(message);
			FunctionCode requestCode = serializer.ReadFunctionCodeFromHeader();
			if (modbusWriteFunction.TryGetValue(requestCode, out IModbusWriteCommand writeCommand))
			{
				ExecuteWrite(writeCommand, serializer);
			}
			else if (modbusReadFunctions.TryGetValue(requestCode, out IModbusReadCommand readCommand))
			{
				ExecuteRead(readCommand, serializer);
			}
		}

		public async void ExecuteRead(IModbusReadCommand command, ITcpSerializer serializer)
		{
			command.SetParams(serializer);
			
			try
			{
				await command.Execute();
				byte[] securedMessage = security.Secure(serializer.Message);
				proxy.Communication.Send(securedMessage);
			}
			catch (Exception e)
			{
				Console.WriteLine("Modbus read operation failed! " + e.Message);
            }
		}

		public void ExecuteWrite(IModbusWriteCommand command, ITcpSerializer serializer)
		{
			command.SetParams(serializer);
			command.Execute();
		}
	}
}

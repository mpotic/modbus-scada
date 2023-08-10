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

		private ITcpSerializer serializer;

		private ITcpConnection proxy;

		private ISecurity security;

		public ProxyToSlaveMessageCommand(IModbusConnection slave, ISecurity security = null)
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

		public void SetParams(IConnection connection, ITcpSerializer serializer)
		{
			proxy = (ITcpConnection)connection;
			this.serializer = serializer;
		}

		public void Execute()
		{
			FunctionCode requestCode = serializer.ReadFunctionCodeFromHeader();

			if (modbusWriteFunction.TryGetValue(requestCode, out IModbusWriteCommand writeCommand))
			{
				ExecuteWrite(writeCommand);
			}
			else if (modbusReadFunctions.TryGetValue(requestCode, out IModbusReadCommand readCommand))
			{
				ExecuteRead(readCommand);
			}
		}

		public async void ExecuteRead(IModbusReadCommand command)
		{
			command.SetParams(serializer);
			
			try
			{
				await command.Execute();
				proxy.Communication.Send(serializer.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine("Modbus read operation failed! " + e.Message);
            }
		}

		public void ExecuteWrite(IModbusWriteCommand command)
		{
			command.SetParams(serializer);
			command.Execute();
		}
	}
}

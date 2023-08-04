using Common.Connection;
using Common.Enums;
using Proxy.Connections;
using System.Collections.Generic;

namespace Proxy.Commands
{
	class ProxyToSlaveMessageCommand : IMessageCommand
	{
		private readonly Dictionary<FunctionCode, IModbusCommand> modbusFunctions;

        private ITcpSerializer serializer;

        private ITcpConnection proxy;

        private readonly IModbusConnection slave;

		public ProxyToSlaveMessageCommand(IModbusConnection slave)
		{
            this.slave = slave;
            modbusFunctions = new Dictionary<FunctionCode, IModbusCommand>
            {
                { FunctionCode.ReadCoils, new ReadCoilCommand(slave) },
                { FunctionCode.ReadHolding, new ReadHoldingCommand(slave) },
                { FunctionCode.ReadDiscreteInputs, new ReadDiscreteInputCommand(slave) },
                { FunctionCode.ReadAnalogInputs, new ReadAnalogInputCommand(slave) },
                { FunctionCode.WriteCoils, new WriteCoilCommand(slave) },
                { FunctionCode.WriteHolding, new WriteHoldingCommand(slave) }
            };
        }

        public void SetParams(IConnection connection, ITcpSerializer serializer)
        {
            proxy = (ITcpConnection)connection;
            this.serializer = serializer;
        }

        public void Execute()
		{
            FunctionCode requestCode = serializer.ReadFunctionCodeFromHeader();
            modbusFunctions[requestCode].SetParams(proxy, serializer);
			modbusFunctions[requestCode].Execute();
        }
	}
}

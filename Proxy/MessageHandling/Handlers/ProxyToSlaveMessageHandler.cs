using Common.Connection;
using Common.Enums;
using NModbus;
using Proxy.MessageHandling.Handlers.ModbusRequestHandlers;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Proxy.MessageHandling
{
    class ProxyToSlaveMessageHandler : IMessageHandler
	{
		private readonly Dictionary<FunctionCode, IModbusRequestHandler> requestHandling;

        private readonly int slavePort = 502;

        private readonly ITcpSerializer serializer;

        private readonly ITcpSocketHandler proxySocket;

        private IModbusMaster slave;

		public ProxyToSlaveMessageHandler(ITcpSerializer serializer, ITcpSocketHandler socket)
		{
			this.serializer = serializer;
            proxySocket = socket;

            requestHandling = new Dictionary<FunctionCode, IModbusRequestHandler>
            {
                { FunctionCode.ReadCoils, new ReadCoilHandler(serializer, proxySocket) },
                { FunctionCode.ReadHolding, new ReadHoldingHandler(serializer, proxySocket) },
                { FunctionCode.ReadDiscreteInputs, new ReadDiscreteInputHandler(serializer, proxySocket) },
                { FunctionCode.ReadAnalogInputs, new ReadAnalogInputHandler(serializer, proxySocket) },
                { FunctionCode.WriteCoils, new WriteCoilHandler(serializer) },
                { FunctionCode.WriteHolding, new WriteHoldingHandler(serializer) }
            };
        }

        public void Process()
		{
            CheckModbusSlaveConnection();
            FunctionCode requestCode = serializer.ReadFunctionCodeFromHeader();
            requestHandling[requestCode].Process(slave);
        }

        private void CheckModbusSlaveConnection()
        {
            if(slave != null)
            {
                return;
            }

            TcpClient tcpClient = new TcpClient();
            tcpClient.Client.Connect(IPAddress.Loopback, slavePort);
            IModbusFactory factory = new ModbusFactory();
            slave = factory.CreateMaster(tcpClient);
        }
	}
}

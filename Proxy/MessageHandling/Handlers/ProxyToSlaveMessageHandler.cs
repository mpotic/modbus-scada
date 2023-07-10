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
		private readonly Dictionary<ModbusRequestCode, IModbusRequestHandler> requestHandling;

        private readonly int slavePort = 502;

        private readonly ITcpSerializer serializer;

        private readonly ITcpSocketHandler proxySocket;

        private IModbusMaster slave;

		public ProxyToSlaveMessageHandler(ITcpSerializer serializer, ITcpSocketHandler socket)
		{
			this.serializer = serializer;
            proxySocket = socket;

            requestHandling = new Dictionary<ModbusRequestCode, IModbusRequestHandler>
            {
                { ModbusRequestCode.ReadCoil, new ReadCoilHandler(serializer, proxySocket) },
                { ModbusRequestCode.ReadHolding, new ReadHoldingHandler(serializer, proxySocket) },
                { ModbusRequestCode.ReadDiscreteInput, new ReadDiscreteInputHandler(serializer, proxySocket) },
                { ModbusRequestCode.ReadAnalogInput, new ReadAnalogInputHandler(serializer, proxySocket) },
                { ModbusRequestCode.WriteCoil, new WriteCoilHandler(serializer) },
                { ModbusRequestCode.WriteHolding, new WriteHoldingHandler(serializer) }
            };
        }

        public void Process()
		{
            CheckModbusSlaveConnection();
            ModbusRequestCode requestCode = serializer.ReadRequestCodeFromHeader();
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

﻿using Common.Connection;
using Common.DTO;
using Common.Enums;
using Proxy.Connections;

namespace Proxy.Commands
{
	internal class ReadDiscreteInputCommand : IModbusCommand
	{
		private ITcpSerializer serializer;

		private ITcpConnection proxy;

		private readonly IModbusConnection slave;

		public ReadDiscreteInputCommand(IModbusConnection slave)
		{
			this.slave = slave;
		}

		public void SetParams(IConnection connection, ITcpSerializer serializer)
		{
			this.serializer = serializer;
			proxy = (ITcpConnection)connection;
		}

		public async void Execute()
        {
            byte slaveAddress = serializer.ReadSlaveAddressFromBody();
            ushort startAddress = serializer.ReadStartAddressFromBody();
            ushort numberOfPoints = serializer.ReadNumberOfPointsFromBody();
			IReadParams readParams = new ReadParams(slaveAddress, startAddress, numberOfPoints);
            IReadDiscreteResponse response = await slave.Rtu.ReadDiscreteInput(readParams);

            SendResponse(response.BoolValues);
        }

        private void SendResponse(bool[] values)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.ProxyToMaster, FunctionCode.ReadDiscreteInputs);
            serializer.AddBody(values);
            proxy.Communication.Send(serializer.Message);
        }
    }
}
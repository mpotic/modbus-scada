﻿using Common.Connection;
using Common.DTO;
using Common.Enums;
using Proxy.Connections;
using System;
using System.Threading.Tasks;

namespace Proxy.Commands
{
	internal class ReadAnalogInputCommand : IModbusReadCommand
    {
        private ITcpSerializer serializer;

        private readonly IModbusConnection slave;

        public ReadAnalogInputCommand(IModbusConnection slave)
        {
            this.slave = slave;
        }

        public void SetParams(ITcpSerializer serializer)
        {
            this.serializer = serializer;
        }

        public async Task Execute()
        {
            byte slaveAddress = serializer.ReadSlaveAddressFromBody();
            ushort startAddress = serializer.ReadStartAddressFromBody();
            ushort numberOfPoints = serializer.ReadNumberOfPointsFromBody();
            IReadParams readParams = new ReadParams(slaveAddress, startAddress, numberOfPoints);

            try
            {
				IReadAnalogResponse response = await slave.Rtu.ReadAnalogInput(readParams);
				FormatResponse(response.Values);
			}
            catch(Exception e)
            {
                throw e;
            }
        }

        private void FormatResponse(ushort[] values)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.ProxyToMaster, FunctionCode.ReadAnalogInputs);
            serializer.AddBody(values);
			serializer.AddSizeToHeader();
        }
	}
}

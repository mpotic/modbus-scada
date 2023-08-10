﻿using Common.Connection;
using Common.DTO;
using Common.Enums;
using Proxy.Connections;
using System;
using System.Threading.Tasks;

namespace Proxy.Commands
{
	internal class ReadDiscreteInputCommand : IModbusReadCommand
	{
		private ITcpSerializer serializer;

		private readonly IModbusConnection slave;

		public ReadDiscreteInputCommand(IModbusConnection slave)
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
				IReadDiscreteResponse response = await slave.Rtu.ReadDiscreteInput(readParams);
				FormatResponse(response.BoolValues);
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		private void FormatResponse(bool[] values)
		{
			serializer.InitMessage();
			serializer.AddHeader(SenderCode.ProxyToMaster, FunctionCode.ReadDiscreteInputs);
			serializer.AddBody(values);
			serializer.AddSizeToHeader();
		}
	}
}

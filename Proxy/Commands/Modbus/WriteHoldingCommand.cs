using Common.Connection;
using Common.DTO;
using Proxy.Connections;
using System;

namespace Proxy.Commands
{
	internal class WriteHoldingCommand : IModbusWriteCommand
	{
		private ITcpSerializer serializer;

		private readonly IModbusConnection slave;

		public WriteHoldingCommand(IModbusConnection slave)
		{
			this.slave = slave;
		}

		public void SetParams(ITcpSerializer serializer)
		{
			this.serializer = serializer;
		}

		public void Execute()
		{
			byte slaveAddress = serializer.ReadSlaveAddressFromBody();
			ushort startAddress = serializer.ReadStartAddressFromBody();
			ushort[] writeValues = serializer.ReadHoldingWriteValuesFromBody();

			try
			{
				IWriteHoldingParams writeParams = new WriteHoldingParams(slaveAddress, startAddress, writeValues);
				IResponse response = slave.Rtu.WriteHolding(writeParams);
			}
			catch (Exception e)
			{
                Console.WriteLine("Modbus write operation failed! " + e.Message);
            }
		}
	}
}

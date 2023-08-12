using Common.Connection;
using Common.DTO;
using Proxy.Connections;
using System;

namespace Proxy.Commands
{
	internal class WriteCoilCommand : IModbusWriteCommand
	{
		private ITcpSerializer serializer;

		private readonly IModbusConnection slave;

		public WriteCoilCommand(IModbusConnection slave)
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
			byte[] writeValues = serializer.ReadDiscreteValuesFromBody();

			try
			{
				IWriteCoilParams writeParams = new WriteCoilParams(slaveAddress, startAddress, writeValues);
				slave.Rtu.WriteCoil(writeParams);
			}
			catch (Exception e)
			{
                Console.WriteLine("Modbus write operation failed! " + e.Message);
            }
		}
	}
}

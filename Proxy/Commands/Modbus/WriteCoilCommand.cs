using Common.Connection;
using Common.DTO;
using Proxy.Connections;

namespace Proxy.Commands
{
	internal class WriteCoilCommand : IModbusCommand
	{
		private ITcpSerializer serializer;

		private readonly IModbusConnection slave;

		public WriteCoilCommand(IModbusConnection slave)
		{
			this.slave = slave;
		}

		public void SetParams(IConnection connection, ITcpSerializer serializer)
		{
			this.serializer = serializer;
		}

		public void Execute()
		{
			byte slaveAddress = serializer.ReadSlaveAddressFromBody();
			ushort startAddress = serializer.ReadStartAddressFromBody();
			bool[] writeValues = serializer.ReadCoilWriteValuesFromBody();
			IWriteCoilParams writeParams = new WriteCoilParams(slaveAddress, startAddress, writeValues);
			slave.Rtu.WriteCoil(writeParams);
		}
	}
}

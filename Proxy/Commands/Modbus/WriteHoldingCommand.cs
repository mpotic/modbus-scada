using Common.Connection;
using Common.DTO;
using Proxy.Connections;

namespace Proxy.Commands
{
	internal class WriteHoldingCommand : IModbusCommand
	{
		private ITcpSerializer serializer;

		private readonly IModbusConnection slave;

		public WriteHoldingCommand(IModbusConnection slave)
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
            ushort[] writeValues = serializer.ReadHoldingWriteValuesFromBody();
			IWriteHoldingParams writeParams = new WriteHoldingParams(slaveAddress, startAddress, writeValues);
            IResponse response = slave.Rtu.WriteHolding(writeParams);
        }
	}
}

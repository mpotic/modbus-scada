using ModbusServices.Connection;

namespace ModbusServices.ServiceProviders
{
	internal class StandardService : IStandardService
	{
		private IStandardConnection connection;

		public StandardService(IStandardConnection connection)
		{
			this.connection = connection;
		}

		public ushort[] ReadAnalogInput(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			throw new System.NotImplementedException();
		}

		public bool[] ReadCoil(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			throw new System.NotImplementedException();
		}

		public bool[] ReadDiscreteInput(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			throw new System.NotImplementedException();
		}

		public ushort[] ReadHolding(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			throw new System.NotImplementedException();
		}

		public void WriteCoil(byte slaveAddress, ushort startAddress, bool[] writeValues)
		{
			throw new System.NotImplementedException();
		}

		public void WriteHolding(byte slaveAddress, ushort startAddress, ushort[] writeValues)
		{
			throw new System.NotImplementedException();
		}
	}
}

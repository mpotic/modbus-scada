using ModbusServices.Connection;

namespace ModbusServices.ServiceProviders
{
	internal class ModbusService : IModbusService
	{
		private IModbusConnection connection;

		public ModbusService(IModbusConnection connection)
		{
			this.connection = connection;
		}

		public ushort[] ReadHolding(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			ushort[] response = connection.ModbusMaster.ReadHoldingRegisters(slaveAddress, startAddress, numberOfPoints);

			return response;
		}

		public ushort[] ReadAnalogInput(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			ushort[] response = connection.ModbusMaster.ReadInputRegisters(slaveAddress, startAddress, numberOfPoints);

			return response;
		}

		public bool[] ReadCoil(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			bool[] response = connection.ModbusMaster.ReadCoils(slaveAddress, startAddress, numberOfPoints);

			return response;
		}

		public bool[] ReadDiscreteInput(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			bool[] response = connection.ModbusMaster.ReadInputs(slaveAddress, startAddress, numberOfPoints);

			return response;
		}

		public void WriteHolding(byte slaveAddress, ushort startAddress, ushort[] writeValues)
		{
			connection.ModbusMaster.WriteMultipleRegisters(slaveAddress, startAddress, writeValues);
		}

		public void WriteCoil(byte slaveAddress, ushort startAddress, bool[] writeValues)
		{
			connection.ModbusMaster.WriteMultipleCoils(slaveAddress, startAddress, writeValues);
		}
	}
}

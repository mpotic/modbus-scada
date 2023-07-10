using ModbusServices.Connection;
using System.Threading.Tasks;

namespace ModbusServices.ServiceProviders
{
	internal class ModbusService : IModbusService
	{
		private IModbusConnection connection;

		public ModbusService(IModbusConnection connection)
		{
			this.connection = connection;
		}

		public async Task<ushort[]> ReadHolding(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			ushort[] response = await connection.ModbusMaster.ReadHoldingRegistersAsync(slaveAddress, startAddress, numberOfPoints);

			return response;
		}

		public async Task<ushort[]> ReadAnalogInput(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			ushort[] response = await connection.ModbusMaster.ReadInputRegistersAsync(slaveAddress, startAddress, numberOfPoints);

			return response;
		}

		public async Task<bool[]> ReadCoil(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			bool[] response = await connection.ModbusMaster.ReadCoilsAsync(slaveAddress, startAddress, numberOfPoints);

			return response;
		}

		public async Task<bool[]> ReadDiscreteInput(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			bool[] response = await connection.ModbusMaster.ReadInputsAsync(slaveAddress, startAddress, numberOfPoints);

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

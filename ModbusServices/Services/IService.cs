using System.Threading.Tasks;

namespace ModbusServices.ServiceProviders
{
	/// <summary>
	/// Generic specification for the requests made to the Modbus slave.
	/// </summary>
	interface IService
	{
		Task<ushort[]> ReadHolding(byte slaveAddress, ushort startAddress, ushort numberOfPoints);

		Task<ushort[]> ReadAnalogInput(byte slaveAddress, ushort startAddress, ushort numberOfPoints);

		Task<bool[]> ReadCoil(byte slaveAddress, ushort startAddress, ushort numberOfPoints);

		Task<bool[]> ReadDiscreteInput(byte slaveAddress, ushort startAddress, ushort numberOfPoints);

		void WriteHolding(byte slaveAddress, ushort startAddress, ushort[] writeValues);

		void WriteCoil(byte slaveAddress, ushort startAddress, bool[] writeValues);
	}
}

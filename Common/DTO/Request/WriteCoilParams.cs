using Common.Enums;
using Common.Util;

namespace Common.DTO
{
	public sealed class WriteCoilParams : IWriteCoilParams
	{
		public WriteCoilParams(byte slaveAddress, ushort startAddress, byte[] writeValues, ServiceTypeCode serviceType)
		{
			ServiceType = serviceType;
			IByteArrayConverter converter = new ByteArrayConverter();
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			WriteValues = converter.ConvertToBoolArray(writeValues);
		}

		public WriteCoilParams(byte slaveAddress, ushort startAddress, bool[] writeValues, ServiceTypeCode serviceType)
		{
			ServiceType = serviceType;
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			WriteValues = writeValues;
		}

		public WriteCoilParams(byte slaveAddress, ushort startAddress, byte[] writeValues)
		{
			ServiceType = ServiceTypeCode.ModbusService;
			IByteArrayConverter converter = new ByteArrayConverter();
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			WriteValues = converter.ConvertToBoolArray(writeValues);
		}

		public WriteCoilParams(byte slaveAddress, ushort startAddress, bool[] writeValues)
		{
			ServiceType = ServiceTypeCode.ModbusService;
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			WriteValues = writeValues;
		}

		public ServiceTypeCode ServiceType { get; set; }

		public byte SlaveAddress { get; set; }
	
		public ushort StartAddress { get; set; }
		
		public bool[] WriteValues { get; set; }
	}
}

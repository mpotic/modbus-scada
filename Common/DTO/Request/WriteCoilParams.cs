using Common.Enums;
using Common.Util;

namespace Common.DTO
{
	public sealed class WriteCoilParams : IWriteCoilParams
	{
		public WriteCoilParams(byte slaveAddress, ushort startAddress, byte[] writeValues, ServiceTypeCode serviceType)
		{
			ServiceType = serviceType;
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			ByteWriteValues = writeValues;
		}

		public WriteCoilParams(byte slaveAddress, ushort startAddress, bool[] writeValues)
		{
			IByteArrayConverter converter = new ByteArrayConverter();
			ServiceType = ServiceTypeCode.ModbusService;
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			ByteWriteValues = converter.ConvertToByteArray(writeValues);
		}

		public WriteCoilParams(byte slaveAddress, ushort startAddress, byte[] writeValues)
		{
			IByteArrayConverter converter = new ByteArrayConverter();
			ServiceType = ServiceTypeCode.ModbusService;
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			ByteWriteValues = writeValues;
		}

		public ServiceTypeCode ServiceType { get; set; }

		public byte SlaveAddress { get; set; }

		public ushort StartAddress { get; set; }

		public byte[] ByteWriteValues { get; set; }

		public bool[] BoolWriteValues
		{
			get
			{
				IByteArrayConverter converter = new ByteArrayConverter();
				return converter.ConvertToBoolArray(ByteWriteValues);
			}
		}
	}
}

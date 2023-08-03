using Common.Enums;

namespace Common.DTO
{
	public sealed class WriteHoldingParams : IWriteHoldingParams
	{
		public WriteHoldingParams(byte slaveAddress, ushort startAddress, ushort[] writeValues)
		{
			ServiceType = ServiceTypeCode.ModbusService;
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			WriteValues = writeValues;
		}

		public WriteHoldingParams(byte slaveAddress, ushort startAddress, ushort[] writeValues, ServiceTypeCode serviceType)
		{
			ServiceType = serviceType;
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			WriteValues = writeValues;
		}

		public ServiceTypeCode ServiceType { get; set; }

		public byte SlaveAddress { get; set; }
		
		public ushort StartAddress { get; set; }
		
		public ushort[] WriteValues { get; set; }
	}
}

using Common.Enums;

namespace Common.DTO
{
	public sealed class ReadParams : IReadParams
	{
		public ReadParams(byte slaveAddress, ushort startAddress, ushort numberOfPoints, ServiceTypeCode serviceType)
		{
			ServiceType = serviceType;
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			NumberOfPoints = numberOfPoints;
		}

		public ReadParams(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			ServiceType = ServiceTypeCode.ModbusService;
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			NumberOfPoints = numberOfPoints;
		}

		public ServiceTypeCode ServiceType { get; set; }

		public byte SlaveAddress { get; set; }

		public ushort StartAddress { get; set; }

		public ushort NumberOfPoints { get; set; }
	}
}

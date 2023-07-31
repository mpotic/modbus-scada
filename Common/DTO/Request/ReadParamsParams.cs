namespace Common.DTO
{
	public class ReadParamsParams : IReadParams
	{
		public ReadParamsParams(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			NumberOfPoints = numberOfPoints;
		}

		public byte SlaveAddress { get; set; }

		public ushort StartAddress { get; set; }

		public ushort NumberOfPoints { get; set; }
	}
}

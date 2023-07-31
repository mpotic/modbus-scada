namespace Common.DTO
{
	public class WriteAnalogParams : IWriteHoldingParams
	{
		public WriteAnalogParams(byte slaveAddress, ushort startAddress, ushort[] writeValues)
		{
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			WriteValues = writeValues;
		}

		public byte SlaveAddress { get; set; }
		
		public ushort StartAddress { get; set; }
		
		public ushort[] WriteValues { get; set; }
	}
}

namespace Common.ActionDto
{
	public class ModbusActionParams : IModbusActionParams
	{
		public ModbusActionParams(ushort startAddress)
		{
			StartAddress = startAddress;
			SlaveAddress = 1;
			NumberOfPoints = 1;
		}

		public ModbusActionParams(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			NumberOfPoints = numberOfPoints;
		}

		public ModbusActionParams(byte slaveAddress, ushort startAddress, byte[] coilWriteValues)
		{
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			CoilWriteValues = coilWriteValues;
		}

		public ModbusActionParams(byte slaveAddress, ushort startAddress, ushort[] holdingWriteValues)
		{
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			HoldingWriteValues = holdingWriteValues;
		}

		public byte SlaveAddress { get; set; }

		public ushort StartAddress { get; set; }

		public ushort NumberOfPoints { get; set; }

		public byte[] CoilWriteValues { get; set; }

		public ushort[] HoldingWriteValues { get; set; }
	}
}

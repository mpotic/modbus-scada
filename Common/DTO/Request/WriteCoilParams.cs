using Common.Util;

namespace Common.DTO
{
	public class WriteCoilParams : IWriteCoilParams
	{
		public WriteCoilParams(byte slaveAddress, ushort startAddress, byte[] writeValues)
		{
			IByteArrayConverter converter = new ByteArrayConverter();
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			WriteValues = converter.ConvertToBoolArray(writeValues);
		}

		public WriteCoilParams(byte slaveAddress, ushort startAddress, bool[] writeValues)
		{
			SlaveAddress = slaveAddress;
			StartAddress = startAddress;
			WriteValues = writeValues;
		}

		public byte SlaveAddress { get; set; }
	
		public ushort StartAddress { get; set; }
		
		public bool[] WriteValues { get; set; }
	}
}

namespace Common.DTO
{
	public interface IWriteCoilParams : IParams
	{
		byte SlaveAddress { get; set; }

		ushort StartAddress { get; set; }

		byte[] ByteWriteValues { get; set; }

		bool[] BoolWriteValues { get; }
	}
}

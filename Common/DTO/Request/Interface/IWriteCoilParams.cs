namespace Common.DTO
{
	public interface IWriteCoilParams
	{
		byte SlaveAddress { get; set; }

		ushort StartAddress { get; set; }

		bool[] WriteValues { get; set; }
	}
}

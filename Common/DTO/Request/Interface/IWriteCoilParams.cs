namespace Common.DTO
{
	public interface IWriteCoilParams : IParams
	{
		byte SlaveAddress { get; set; }

		ushort StartAddress { get; set; }

		bool[] WriteValues { get; set; }
	}
}

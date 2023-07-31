namespace Common.DTO
{
	public interface IReadParams
	{
		byte SlaveAddress { get; set; }

		ushort StartAddress { get; set; }

		ushort NumberOfPoints { get; set; }
	}
}

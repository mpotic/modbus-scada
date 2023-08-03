namespace Common.DTO
{
	public interface IReadParams : IParams
	{
		byte SlaveAddress { get; set; }

		ushort StartAddress { get; set; }

		ushort NumberOfPoints { get; set; }
	}
}

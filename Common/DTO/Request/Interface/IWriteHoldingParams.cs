namespace Common.DTO
{
	public interface IWriteHoldingParams : IParams
	{
		byte SlaveAddress { get; set; }

		ushort StartAddress { get; set; }

		ushort[] WriteValues { get; set; }
	}
}

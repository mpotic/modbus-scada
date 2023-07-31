namespace Common.DTO
{
	public interface IWriteHoldingParams
	{
		byte SlaveAddress { get; set; }

		ushort StartAddress { get; set; }

		ushort[] WriteValues { get; set; }
	}
}

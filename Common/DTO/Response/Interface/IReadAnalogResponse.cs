namespace Common.DTO
{
	public interface IReadAnalogResponse : IResponse
	{
		ushort[] Values { get; set; }
	}
}

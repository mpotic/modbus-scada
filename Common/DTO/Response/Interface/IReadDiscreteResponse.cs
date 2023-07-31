namespace Common.DTO
{
	public interface IReadDiscreteResponse : IResponse
	{
		bool[] BoolValues { get; }

		byte[] ByteValues { get; }
	}
}

namespace Common.DTO
{
	public interface ITcpReceiveResponse : IResponse
	{
		byte[] Payload { get; }
	}
}

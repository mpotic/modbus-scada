namespace Common.DTO
{
	public sealed class TcpReceiveResponse : ITcpReceiveResponse
	{
		public TcpReceiveResponse(bool isSuccessful, byte[] payload)
		{
			Payload = payload;
			IsSuccessful = isSuccessful;
		}

		public TcpReceiveResponse(bool isSuccessful, string errorMessage)
		{
			IsSuccessful = isSuccessful;
			ErrorMessage = errorMessage;
		}

		public byte[] Payload { get; }

		public bool IsSuccessful { get; }

		public string ErrorMessage { get; }
	}
}

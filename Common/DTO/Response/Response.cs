namespace Common.DTO
{
	public sealed class Response : IResponse
	{
		public Response(bool isSuccessful)
		{
			IsSuccessful = isSuccessful;
		}

		public Response(bool isSuccessful, string errorMessage)
		{
			IsSuccessful = isSuccessful;
			ErrorMessage = errorMessage;
		}

		public bool IsSuccessful { get; }

		public string ErrorMessage { get; }
	}
}

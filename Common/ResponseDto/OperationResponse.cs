namespace Common.ResponseDto
{
	public class OperationResponse : IOperationResponse
	{
		public OperationResponse(bool isSuccessfull)
		{
			IsSuccessfull = isSuccessfull;
		}

		public OperationResponse(bool isSuccessfull, string message)
		{
			IsSuccessfull = isSuccessfull;
			Message = message;
		}

		public bool IsSuccessfull { get; set; }

		public string Message { get; set; }
	}
}

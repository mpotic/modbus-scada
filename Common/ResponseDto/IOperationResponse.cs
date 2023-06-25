namespace Common.ResponseDto
{
	public interface IOperationResponse
	{
		bool IsSuccessfull { get; set; }

		string Message { get; set; }
	}
}

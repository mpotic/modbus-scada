namespace Common.DTO
{
	public interface IResponse
	{
		bool IsSuccessful { get; }

		string ErrorMessage { get; }
	}
}

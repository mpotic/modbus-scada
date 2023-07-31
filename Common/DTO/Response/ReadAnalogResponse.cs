namespace Common.DTO
{
	public sealed class ReadAnalogResponse : IReadAnalogResponse
	{
		public ReadAnalogResponse(ushort[] values)
		{
			Values = values;
			IsSuccessful = true;
		}

		public ReadAnalogResponse(bool isSuccessful, string errorMessage)
		{
			IsSuccessful = isSuccessful;
			ErrorMessage = errorMessage;
		}

		public ushort[] Values { get; set; }

		public bool IsSuccessful { get; }

		public string ErrorMessage { get; }
	}
}

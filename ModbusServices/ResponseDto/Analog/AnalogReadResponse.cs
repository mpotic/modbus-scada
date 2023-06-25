namespace ModbusServices.ResponseDto.Analog
{
	public class AnalogReadResponse : IAnalogReadResponse
	{
		public AnalogReadResponse(bool isSuccessfull, string errorMessage)
		{
			IsSuccessfull = isSuccessfull;
			Message = errorMessage;
		}

		public AnalogReadResponse(bool isSuccessfull, ushort[] values)
		{
			IsSuccessfull = isSuccessfull;
			Values = values;
		}

		public bool IsSuccessfull { get; set; }

		public string Message { get; set; }

		public ushort[] Values { get; set; }
	}
}

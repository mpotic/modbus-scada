namespace ModbusServices.ResponseDto.Discrete
{
	public class DiscreteReadResponse : IDiscreteReadResponse
	{
		public DiscreteReadResponse(bool isSuccessfull, byte[] values)
		{
			IsSuccessfull = isSuccessfull;
			Values = values;
		}

		public DiscreteReadResponse(bool isSuccessfull, string errorMessage)
		{
			IsSuccessfull = isSuccessfull;
			Message = errorMessage;
		}

		public bool IsSuccessfull { get; set; }

		public byte[] Values { get; set; }

		public string Message { get; set; }
	}
}

using Common.Util;

namespace Common.DTO
{
	public sealed class ReadDiscreteResponse : IReadDiscreteResponse
	{
		private readonly IByteArrayConverter converter = new ByteArrayConverter();

		public ReadDiscreteResponse(byte[] byteValues)
		{
			BoolValues = converter.ConvertToBoolArray(byteValues);
			IsSuccessful = true;
		}

		public ReadDiscreteResponse(bool[] boolValues)
		{
			BoolValues = boolValues;
			IsSuccessful = true;
		}

		public ReadDiscreteResponse(bool isSuccessful, string errorMessage)
		{
			IsSuccessful = isSuccessful;
			ErrorMessage = errorMessage;
		}

		public bool[] BoolValues { private set; get; }

		public byte[] ByteValues
		{
			get
			{
				return converter.ConvertToByteArray(BoolValues);
			}
		}

		public bool IsSuccessful { get; }

		public string ErrorMessage { get; }
	}
}

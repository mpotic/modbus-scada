namespace ModbusApi
{
	public interface IMessageBoxCallback
	{
		void DisplayError(string message);

		void DisplayInformation(string message);

		void DisplayWarning(string message);
	}
}

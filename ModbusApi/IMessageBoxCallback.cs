namespace ModbusApi
{
	public interface IMessageBoxCallback
	{
		void DisplayError(string message);

		void DisplaySuccess(string message);
	}
}

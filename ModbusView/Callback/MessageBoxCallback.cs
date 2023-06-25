using ModbusApi;
using System.Windows;

namespace ModbusView.Callback
{
	public class MessageBoxCallback : IMessageBoxCallback
	{
		public void DisplayError(string message)
		{
			MessageBox.Show(message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public void DisplaySuccess(string message)
		{
			MessageBox.Show(message, "Exception", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}

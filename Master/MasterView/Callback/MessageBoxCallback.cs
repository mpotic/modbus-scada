﻿using MasterApi;
using System.Windows;

namespace MasterView.Callback
{
	internal sealed class MessageBoxCallback : IMessageBoxCallback
	{
		public void DisplayError(string message)
		{
			MessageBox.Show(message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public void DisplayInformation(string message)
		{
			MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		public void DisplayWarning(string message)
		{
			MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
	}
}

using Common.Callback;
using Common.Enums;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace MasterView.Callback
{
	internal sealed class ConnectionStateCallback : IConnectionStatusCallback
	{
		TextBlock connectionText;

		Ellipse connectionEllipse;

		Dictionary<ConnectionStatusCode, SolidColorBrush> ellipseColors = new Dictionary<ConnectionStatusCode, SolidColorBrush>()
		{ 
			{ ConnectionStatusCode.Disconnected, Brushes.Red },
			{ ConnectionStatusCode.Connected, Brushes.Green },
			{ ConnectionStatusCode.Connecting, Brushes.Yellow },
			{ ConnectionStatusCode.Listening, Brushes.Blue }
		};

		public ConnectionStateCallback(TextBlock text, Ellipse ellipse)
		{
			connectionText = text;
			connectionEllipse = ellipse;
		}

		public void ConenctionStatusChanged(ConnectionStatusCode statusCode)
		{
			connectionText.Text = statusCode.ToString();
			connectionEllipse.Fill = ellipseColors[statusCode];
		}
	}
}

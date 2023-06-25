using Common.ActionDto;
using ModbusApi.Api;
using ModbusView.ModbusActions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ModbusView
{
	/// <summary>
	/// Interaction logic for ConnectUserControl.xaml
	/// </summary>
	public partial class ConnectUserControl : UserControl
	{
		Dictionary<ActionCode, IModbusAction> actionMap = new Dictionary<ActionCode, IModbusAction>();
		
		public ConnectUserControl(IConnectionApi connectionApi)
		{
			InitializeComponent();

			actionMap.Add(ActionCode.ModbusConnect, new ModbusConnectAction(connectionApi));
			actionMap.Add(ActionCode.StandardConnect, new StandardConnectAction(connectionApi));
		}

		private void ModbusConnectButton_Click(object sender, RoutedEventArgs e)
		{
			int serverPort;
			int clientPort;
			IConnectionParams connectionParams;

			try
			{
				serverPort = int.Parse(ServerPort.Text);
				clientPort = int.Parse(ClientPort.Text);

				connectionParams = new ConnectionParams(clientPort, serverPort);

				actionMap[ActionCode.ModbusConnect].SetParams(connectionParams);
				actionMap[ActionCode.ModbusConnect].Execute();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void StandardConnectButton_Click(object sender, RoutedEventArgs e)
		{
			int serverPort;
			int clientPort;
			IConnectionParams connectionParams;

			try
			{
				serverPort = int.Parse(ServerPort.Text);
				clientPort = int.Parse(ClientPort.Text);

				connectionParams = new ConnectionParams(clientPort, serverPort);

				actionMap[ActionCode.StandardConnect].SetParams(connectionParams);
				actionMap[ActionCode.StandardConnect].Execute();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}

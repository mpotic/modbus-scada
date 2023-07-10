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
			int clientPort;
			IConnectionParams connectionParams;

			try
			{
				clientPort = int.Parse(ModbusClientPort.Text);

				connectionParams = new ConnectionParams(clientPort);

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
			int clientPort;
			IConnectionParams connectionParams;

			try
			{
				clientPort = int.Parse(StandardClientPort.Text);

				connectionParams = new ConnectionParams(clientPort);

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

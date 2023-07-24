using Common.ParamsDto;
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
		private readonly Dictionary<ActionCode, IModbusAction> actionMap = new Dictionary<ActionCode, IModbusAction>();

		private readonly IConnectionApi connectionApi;
		
		public ConnectUserControl(IConnectionApi connectionApi)
		{
			InitializeComponent();

			actionMap.Add(ActionCode.ModbusConnect, new ModbusConnectAction(connectionApi));
			actionMap.Add(ActionCode.StandardConnect, new StandardConnectAction(connectionApi));
			this.connectionApi = connectionApi;
		}

		private void ModbusConnectButton_Click(object sender, RoutedEventArgs e)
		{
			int clientPort;
			int hostPort;
			IConnectionParams connectionParams;

			try
			{
				clientPort = int.Parse(ModbusClientPort.Text);
				hostPort = int.Parse(ModbusHostPort.Text);

				connectionParams = new ConnectionParams(clientPort, hostPort);

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
			int hostPort;
			IConnectionParams connectionParams;

			try
			{
				clientPort = int.Parse(StandardClientPort.Text);
				hostPort = int.Parse(StandardHostPort.Text);

				connectionParams = new ConnectionParams(clientPort, hostPort);

				actionMap[ActionCode.StandardConnect].SetParams(connectionParams);
				actionMap[ActionCode.StandardConnect].Execute();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void Disconnect_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				connectionApi.Disconnect();
			}
			catch(Exception exception)
			{
				MessageBox.Show(exception.Message, "Disconnect error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}

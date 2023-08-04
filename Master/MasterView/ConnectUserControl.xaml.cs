using MasterApi.Api;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Common.DTO;
using MasterView.Actions;
using MasterView.Callback;

namespace MasterView
{
	/// <summary>
	/// Interaction logic for ConnectUserControl.xaml
	/// </summary>
	public partial class ConnectUserControl : UserControl
	{
		private readonly Dictionary<ActionCode, IConnectionAction> actions;
		
		public ConnectUserControl(IConnectionApi connectionApi)
		{
			InitializeComponent();

			actions = new Dictionary<ActionCode, IConnectionAction>()
			{
				{ ActionCode.Connect, new ConnectAction(connectionApi) },
				{ ActionCode.Disconnect, new DisconnectAction(connectionApi)}
			};
			ConnectionStateCallback modbusCallback = new ConnectionStateCallback(ModbusStatusTextBlock, ModbusStatusEllipse);
			ConnectionStateCallback tcpCallback = new ConnectionStateCallback(TcpStatusTextBlock, TcpStatusEllipse);
			connectionApi.RegisterConnectionStatusCallback(modbusCallback, ServiceTypeCode.ModbusService);
			connectionApi.RegisterConnectionStatusCallback(tcpCallback, ServiceTypeCode.TcpService);
		}

		private void ModbusConnectButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int clientPort = int.Parse(ModbusClientPort.Text);
				int hostPort = int.Parse(ModbusHostPort.Text);
				IConnectionParams connectionParams = new ConnectionParams(clientPort, hostPort, ServiceTypeCode.ModbusService);
				actions[ActionCode.Connect].SetParams(connectionParams);
				actions[ActionCode.Connect].Execute();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void TcpConnectButton_Click(object sender, RoutedEventArgs e)
		{
			int clientPort;
			int hostPort;
			IConnectionParams connectionParams;

			try
			{
				clientPort = int.Parse(StandardClientPort.Text);
				hostPort = int.Parse(StandardHostPort.Text);
				connectionParams = new ConnectionParams(clientPort, hostPort, ServiceTypeCode.TcpService);
				actions[ActionCode.Connect].SetParams(connectionParams);
				actions[ActionCode.Connect].Execute();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ModbusDisconnectButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				IConnectionParams connectionParams = new ConnectionParams(ServiceTypeCode.ModbusService);
				actions[ActionCode.Disconnect].SetParams(connectionParams);
				actions[ActionCode.Disconnect].Execute();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Disconnect error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void TcpDisconnectButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				IConnectionParams connectionParams = new ConnectionParams(ServiceTypeCode.TcpService);
				actions[ActionCode.Disconnect].SetParams(connectionParams);
				actions[ActionCode.Disconnect].Execute();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Disconnect error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}

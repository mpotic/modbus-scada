using Common.DTO;
using Common.Enums;
using MasterApi.Api;
using MasterView.Actions;
using MasterView.ModbusActions;
using MasterView.Util;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MasterView
{
	/// <summary>
	/// Interaction logic for WriteUserControl.xaml
	/// </summary>
	public partial class WriteUserControl : UserControl
	{
		readonly Dictionary<ActionCode, IAction> actions;

		readonly IArrayConverter arrayConverter;

		public WriteUserControl(IWriteApi writeApi)
		{
			InitializeComponent();

			actions = new Dictionary<ActionCode, IAction>
			{
				{ ActionCode.WriteCoils, new WriteCoilsAction(writeApi) },
				{ ActionCode.WriteHolding, new WriteHoldingAction(writeApi) }
			};

			arrayConverter = new ArrayConverter();
			ActionComboBox.ItemsSource = new List<ActionCode> { ActionCode.WriteCoils, ActionCode.WriteHolding };
			ServiceTypeComboBox.ItemsSource = Enum.GetValues(typeof(ServiceTypeCode));
			ServiceTypeComboBox.SelectedItem = ServiceTypeCode.TcpService;
		}

		private void WriteButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if ((ActionCode)ActionComboBox.SelectedItem == ActionCode.WriteHolding)
				{
					WriteHolding();
				}
				else
				{
					WriteCoils();
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}

		private void WriteCoils()
		{
			byte slaveAddress = byte.Parse(SlaveAddressTextBox.Text);
			ushort startAddress = ushort.Parse(StartAddressTextBox.Text);
			ServiceTypeCode serviceType = (ServiceTypeCode)ServiceTypeComboBox.SelectedItem;
			byte[] values = arrayConverter.ConvertStringToByteArray(ValuesTextBox.Text);

			IWriteCoilParams actionParams = new WriteCoilParams(slaveAddress, startAddress, values, serviceType);
			IWriteCoilAction action = (IWriteCoilAction)actions[ActionCode.WriteCoils];
			action.SetParams(actionParams);
			action.Execute();
		}

		private void WriteHolding()
		{
			byte slaveAddress = byte.Parse(SlaveAddressTextBox.Text);
			ushort startAddress = ushort.Parse(StartAddressTextBox.Text);
			ServiceTypeCode serviceType = (ServiceTypeCode)ServiceTypeComboBox.SelectedItem;
			ushort[] values = arrayConverter.ConvertStringToUshortArray(ValuesTextBox.Text);

			IWriteHoldingParams actionParams = new WriteHoldingParams(slaveAddress, startAddress, values, serviceType);
			IWriteHoldingAction action = (IWriteHoldingAction)actions[ActionCode.WriteHolding];
			action.SetParams(actionParams);
			action.Execute();
		}
	}
}

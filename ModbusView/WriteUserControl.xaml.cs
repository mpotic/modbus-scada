using Common.ParamsDto;
using ModbusApi.Api;
using ModbusView.ModbusActions;
using ModbusView.Util;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ModbusView
{
	/// <summary>
	/// Interaction logic for WriteUserControl.xaml
	/// </summary>
	public partial class WriteUserControl : UserControl
	{
		readonly Dictionary<ActionCode, IModbusAction> actionComboBoxItemMap;

		readonly IArrayConverter arrayConverter;

		public WriteUserControl(IDiscreteApi discreteApi, IAnalogApi analogApi)
		{
			InitializeComponent();

			actionComboBoxItemMap = new Dictionary<ActionCode, IModbusAction>();
			actionComboBoxItemMap.Add(ActionCode.WriteCoils, new WriteCoilsAction(discreteApi));
			actionComboBoxItemMap.Add(ActionCode.WriteHolding, new WriteHoldingAction(analogApi));

			arrayConverter = new ArrayConverter();
			ActionComboBox.ItemsSource = new List<ActionCode> { ActionCode.WriteCoils, ActionCode.WriteHolding };
		}

		private void WriteButton_Click(object sender, RoutedEventArgs e)
		{
			byte slaveAddress;
			ushort startAddress;
			IModbusActionParams actionParams;

			try
			{
				slaveAddress = byte.Parse(SlaveAddressTextBox.Text);
				startAddress = ushort.Parse(StartAddressTextBox.Text);

				if ((ActionCode)ActionComboBox.SelectedItem == ActionCode.WriteHolding)
				{
					ushort[] values = arrayConverter.ConvertStringToUshortArray(ValuesTextBox.Text);
					actionParams = new ModbusActionParams(slaveAddress, startAddress, values);
				}
				else
				{
					byte[] values = arrayConverter.ConvertStringToByteArray(ValuesTextBox.Text);
					actionParams = new ModbusActionParams(slaveAddress, startAddress, values);
				}

				actionComboBoxItemMap[(ActionCode)ActionComboBox.SelectedItem].SetParams(actionParams);
				actionComboBoxItemMap[(ActionCode)ActionComboBox.SelectedItem].Execute();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}
	}
}

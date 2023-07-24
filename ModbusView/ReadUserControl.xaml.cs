using Common.ParamsDto;
using ModbusApi.Api;
using ModbusApi.ViewModel;
using ModbusView.ModbusActions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ModbusView
{
	/// <summary>
	/// Interaction logic for ReadUserControl.xaml
	/// </summary>
	public partial class ReadUserControl : UserControl
	{
		readonly Dictionary<ActionCode, IModbusAction> actionComboBoxItemMap;

		readonly IReadResultsViewModel readResultsViewModel;

		public ReadUserControl(IDiscreteApi discreteApi, IAnalogApi analogApi, IReadResultsViewModel resultsViewModel)
		{
			InitializeComponent();

			actionComboBoxItemMap = new Dictionary<ActionCode, IModbusAction>();
			actionComboBoxItemMap.Add(ActionCode.ReadDiscreteInputs, new ReadDiscreteInputsAction(discreteApi));
			actionComboBoxItemMap.Add(ActionCode.ReadCoils, new ReadCoilsAction(discreteApi));
			actionComboBoxItemMap.Add(ActionCode.ReadAnalogInputs, new ReadAnalogInputAction(analogApi));
			actionComboBoxItemMap.Add(ActionCode.ReadHolding, new ReadHoldingAction(analogApi));

			ActionComboBox.ItemsSource = new List<ActionCode>()
				{ ActionCode.ReadDiscreteInputs, ActionCode.ReadCoils, ActionCode.ReadAnalogInputs, ActionCode.ReadHolding };

			readResultsViewModel = resultsViewModel;
			ReadResultsTextBlock.DataContext = readResultsViewModel;
			ReadResultsTextBlock.SetBinding(TextBlock.TextProperty, new Binding("ReadResults"));
		}

		private void ReadButton_Click(object sender, RoutedEventArgs e)
		{
			byte slaveAddress;
			ushort startAddress;
			ushort points;
			IModbusActionParams actionParams;

			try
			{
				slaveAddress = byte.Parse(SlaveAddressTextBox.Text);
				startAddress = ushort.Parse(StartAddressTextBox.Text);
				points = ushort.Parse(PointCountTextBox.Text);
				actionParams = new ModbusActionParams(slaveAddress, startAddress, points);

				actionComboBoxItemMap[(ActionCode)ActionComboBox.SelectedItem].SetParams(actionParams);
				actionComboBoxItemMap[(ActionCode)ActionComboBox.SelectedItem].Execute();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}

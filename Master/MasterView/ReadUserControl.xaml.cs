using Common.DTO;
using Common.Enums;
using MasterApi.Api;
using MasterApi.ViewModel;
using MasterView.Actions;
using MasterView.ModbusActions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MasterView
{
	/// <summary>
	/// Interaction logic for ReadUserControl.xaml
	/// </summary>
	public partial class ReadUserControl : UserControl
	{
		readonly Dictionary<ActionCode, IReadAction> actions;

		readonly IReadResultsViewModel readResultsViewModel;

		readonly IReadApi readApi;

		public ReadUserControl(IReadApi readApi, IReadResultsViewModel resultsViewModel)
		{
			InitializeComponent();

			this.readApi = readApi;
			actions = new Dictionary<ActionCode, IReadAction>
			{
				{ ActionCode.ReadDiscreteInputs, new ReadDiscreteInputsAction(readApi) },
				{ ActionCode.ReadCoils, new ReadCoilsAction(readApi) },
				{ ActionCode.ReadAnalogInputs, new ReadAnalogInputAction(readApi) },
				{ ActionCode.ReadHolding, new ReadHoldingAction(readApi) }
			};

			ActionComboBox.ItemsSource = new List<ActionCode>()
				{ ActionCode.ReadDiscreteInputs, ActionCode.ReadCoils, ActionCode.ReadAnalogInputs, ActionCode.ReadHolding };
			ServiceTypeComboBox.ItemsSource = Enum.GetValues(typeof(ServiceTypeCode));
			ServiceTypeComboBox.SelectedItem = ServiceTypeCode.TcpService;

			readResultsViewModel = resultsViewModel;
			ReadResultsTextBlock.DataContext = readResultsViewModel;
			ReadResultsTextBlock.SetBinding(TextBlock.TextProperty, new Binding("ReadResults"));
		}

		private void ReadButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				byte slaveAddress = byte.Parse(SlaveAddressTextBox.Text);
				ushort startAddress = ushort.Parse(StartAddressTextBox.Text);
				ushort points = ushort.Parse(PointCountTextBox.Text);
				ServiceTypeCode serviceType = (ServiceTypeCode)ServiceTypeComboBox.SelectedItem;
				IReadParams actionParams = new ReadParams(slaveAddress, startAddress, points, serviceType);
				
				ActionCode action = (ActionCode)ActionComboBox.SelectedItem;
				actions[action].SetParams(actionParams);
				actions[action].Execute();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			readApi.ClearResults();
		}
	}
}

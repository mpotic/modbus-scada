using ModbusApi;
using ModbusApi.ViewModel;
using ModbusView.Callback;
using System.Windows;

namespace ModbusView
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			IMessageBoxCallback callback = new MessageBoxCallback();
			IReadResultsViewModel readResultsViewModel = new ReadResultsViewModel();
			IApiHandler apiHandler = new ApiHandler(readResultsViewModel, callback);

			ReadTabItem.Content = new ReadUserControl(apiHandler.DiscreteApi, apiHandler.AnalogApi, readResultsViewModel);
			WriteTabItem.Content = new WriteUserControl(apiHandler.DiscreteApi, apiHandler.AnalogApi);
			ConnectTabItem.Content = new ConnectUserControl(apiHandler.ConnectionApi);
		}
	}
}

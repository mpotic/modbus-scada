using MasterApi;
using MasterApi.ViewModel;
using MasterView.Callback;
using System.Windows;

namespace MasterView
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

			ReadTabItem.Content = new ReadUserControl(apiHandler.ReadApi, readResultsViewModel);
			WriteTabItem.Content = new WriteUserControl(apiHandler.WriteApi);
			ConnectTabItem.Content = new ConnectUserControl(apiHandler.ConnectionApi);
		}
	}
}

using System.ComponentModel;

namespace ModbusApi.ViewModel
{
	public class ReadResultsViewModel : INotifyPropertyChanged, IReadResultsViewModel
	{
		private string readResults = "";

		public string ReadResults 
		{ 
			get => readResults;
			set 
			{
				readResults = value;
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ReadResults)));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

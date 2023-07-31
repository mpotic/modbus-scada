using Common.DTO;
using MasterServices.ServiceProviders;

namespace MasterApi.Api
{
	public class ConnectionApi : IConnectionApi
	{
		readonly IConnectionServiceProvider connectionService;

		readonly IMessageBoxCallback messageBoxCallback;
		
		public ConnectionApi(IConnectionServiceProvider connectionService, IMessageBoxCallback messageBoxCallback)
		{
			this.connectionService = connectionService;
			this.messageBoxCallback = messageBoxCallback;
		}

		public void ModbusConnect(IConnectionParams connectionParams)
		{
			IResponse response = connectionService.ModbusConnect(connectionParams);

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);

				return;
			}

			messageBoxCallback.DisplayInformation(string.IsNullOrWhiteSpace(response.ErrorMessage) ? "Modbus connection established!" : response.ErrorMessage);
		}

		public async void StandardConnect(IConnectionParams connectionParams)
		{
			IResponse response = await connectionService.StandardConnect(connectionParams);

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);

				return;
			}

			messageBoxCallback.DisplayInformation(string.IsNullOrWhiteSpace(response.ErrorMessage) ? "Standard connection established!" : response.ErrorMessage);
		}

		public void Disconnect()
		{
			IResponse response = connectionService.Disconnect();

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayWarning(response.ErrorMessage);

				return;
			}

			messageBoxCallback.DisplayInformation(string.IsNullOrWhiteSpace(response.ErrorMessage) ? "Disconnected!" : response.ErrorMessage);
		}
	}
}

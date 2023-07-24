using Common.ParamsDto;
using Common.ResponseDto;
using ModbusServices.ServiceProviders;

namespace ModbusApi.Api
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
			IOperationResponse response = connectionService.ModbusConnect(connectionParams);

			if (!response.IsSuccessfull)
			{
				messageBoxCallback.DisplayError(response.Message);

				return;
			}

			messageBoxCallback.DisplayInformation(string.IsNullOrWhiteSpace(response.Message) ? "Modbus connection established!" : response.Message);
		}

		public async void StandardConnect(IConnectionParams connectionParams)
		{
			IOperationResponse response = await connectionService.StandardConnect(connectionParams);

			if (!response.IsSuccessfull)
			{
				messageBoxCallback.DisplayError(response.Message);

				return;
			}

			messageBoxCallback.DisplayInformation(string.IsNullOrWhiteSpace(response.Message) ? "Standard connection established!" : response.Message);
		}

		public void Disconnect()
		{
			IOperationResponse response = connectionService.Disconnect();

			if (!response.IsSuccessfull)
			{
				messageBoxCallback.DisplayWarning(response.Message);

				return;
			}

			messageBoxCallback.DisplayInformation(string.IsNullOrWhiteSpace(response.Message) ? "Disconnected!" : response.Message);
		}
	}
}

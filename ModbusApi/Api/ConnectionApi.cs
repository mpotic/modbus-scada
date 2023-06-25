using Common.ActionDto;
using Common.ResponseDto;
using ModbusServices.ServiceProviders;

namespace ModbusApi.Api
{
	public class ConnectionApi : IConnectionApi
	{
		readonly IConnectionProvider connectionService;

		readonly IMessageBoxCallback messageBoxCallback;
		
		public ConnectionApi(IConnectionProvider connectionService, IMessageBoxCallback messageBoxCallback)
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

			messageBoxCallback.DisplaySuccess(string.IsNullOrWhiteSpace(response.Message) ? "Modbus connection established!" : response.Message);
		}

		public void StandardConnect(IConnectionParams connectionParams)
		{
			IOperationResponse response = connectionService.StandardConnect(connectionParams);

			if (!response.IsSuccessfull)
			{
				messageBoxCallback.DisplayError(response.Message);
				return;
			}

			messageBoxCallback.DisplaySuccess(string.IsNullOrWhiteSpace(response.Message) ? "Standard connection established!" : response.Message);
		}
	}
}

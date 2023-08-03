using Common.Callback;
using Common.DTO;
using Common.Enums;
using MasterServices;
using System.Collections.Generic;

namespace MasterApi.Api
{
	public class ConnectionApi : IConnectionApi
	{
		readonly IMessageBoxCallback messageBoxCallback;

		Dictionary<ServiceTypeCode, IRtuServiceProvider> services;

		public ConnectionApi(IServiceProviderHandler handler, IMessageBoxCallback messageBoxCallback)
		{
			this.messageBoxCallback = messageBoxCallback;
			services = new Dictionary<ServiceTypeCode, IRtuServiceProvider>()
			{
				{ ServiceTypeCode.ModbusService, handler.ModbusServiceProvider },
				{ ServiceTypeCode.TcpService, handler.TcpServiceProvider }
			};
		}

		public async void Connect(IConnectionParams connectionParams)
		{
			IResponse response = await services[connectionParams.ServiceType].Connect(connectionParams);

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);

				return;
			}

			messageBoxCallback.DisplayInformation(string.IsNullOrWhiteSpace(response.ErrorMessage) ? 
				$"{connectionParams.ServiceType} connection established!" : response.ErrorMessage);
		}

		public void Disconnect(IConnectionParams connectionParams)
		{
			IResponse response = services[connectionParams.ServiceType].Disconnect();

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);

				return;
			}

			messageBoxCallback.DisplayInformation(string.IsNullOrWhiteSpace(response.ErrorMessage) ?
				"Disconnected!" : response.ErrorMessage);
		}

		public void RegisterConnectionStatusCallback(IConnectionStatusCallback callback, ServiceTypeCode serviceType)
		{
			services[serviceType].RegisterCallack(callback);
		}
	}
}

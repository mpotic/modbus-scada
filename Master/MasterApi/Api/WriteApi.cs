using Common.DTO;
using Common.Enums;
using MasterServices;
using System.Collections.Generic;

namespace MasterApi.Api
{
	internal class WriteApi : IWriteApi
	{
		readonly IMessageBoxCallback messageBoxCallback;

		public Dictionary<ServiceTypeCode, IRtuServiceProvider> services;

		public WriteApi(IMessageBoxCallback messageBoxCallback, IServiceProviderHandler handler)
		{
			this.messageBoxCallback = messageBoxCallback;
			services = new Dictionary<ServiceTypeCode, IRtuServiceProvider>()
			{
				{ ServiceTypeCode.ModbusService, handler.ModbusServiceProvider },
				{ ServiceTypeCode.TcpService, handler.TcpServiceProvider }
			};
		}

		public void WriteHolding(IWriteHoldingParams writeParams)
		{
			IResponse response = services[writeParams.ServiceType].WriteHolding(writeParams);

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);
			}
		}

		public void WriteCoil(IWriteCoilParams writeParams)
		{
			IResponse response = services[writeParams.ServiceType].WriteCoil(writeParams);

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);
			}
		}
	}
}

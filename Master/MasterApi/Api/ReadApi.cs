using Common.DTO;
using Common.Enums;
using Common.Util;
using MasterApi.ViewModel;
using MasterServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasterApi.Api
{
	internal class ReadApi : IReadApi
	{
		readonly IReadResultsViewModel readResultsViewModel;

		readonly IMessageBoxCallback messageBoxCallback;

		readonly IResponseValuesFormatter formatter = new ResponseValuesFormatter();

		public Dictionary<ServiceTypeCode, IRtuServiceProvider> services;

		public ReadApi(IReadResultsViewModel readResultsViewModel, IMessageBoxCallback messageBoxCallback, IServiceProviderHandler handler)
		{
			this.readResultsViewModel = readResultsViewModel;
			this.messageBoxCallback = messageBoxCallback;
			services = new Dictionary<ServiceTypeCode, IRtuServiceProvider>()
			{
				{ ServiceTypeCode.ModbusService, handler.ModbusServiceProvider },
				{ ServiceTypeCode.TcpService, handler.TcpServiceProvider }
			};
		}

		private void UpdateViewModel(Array values, ushort startAddress)
		{
			string formattedResponse = formatter.FormatArray(values, startAddress);
			readResultsViewModel.ReadResults = formattedResponse;
		}

		public async void ReadHolding(IReadParams readParams)
		{
			IReadAnalogResponse response = await Task.Run(async () => 
			{
				return await services[readParams.ServiceType].ReadHolding(readParams);
			});

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);

				return;
			}

			UpdateViewModel(response.Values, readParams.StartAddress);
		}

		public async void ReadAnalogInput(IReadParams readParams)
		{
			IReadAnalogResponse response = await Task.Run(async () =>
			{
				return await services[readParams.ServiceType].ReadAnalogInput(readParams);
			});

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);

				return;
			}

			UpdateViewModel(response.Values, readParams.StartAddress);
		}

		public async void ReadCoil(IReadParams readParams)
		{
			IReadDiscreteResponse response = await Task.Run(async () =>
			{
				return await services[readParams.ServiceType].ReadCoil(readParams);
			});

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);

				return;
			}

			UpdateViewModel(response.ByteValues, readParams.StartAddress);
		}

		public async void ReadDiscreteInput(IReadParams readParams)
		{
			IReadDiscreteResponse response = await Task.Run(async () =>
			{
				return await services[readParams.ServiceType].ReadDiscreteInput(readParams);
			});

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);
				return;
			}

			UpdateViewModel(response.ByteValues, readParams.StartAddress);
		}

		public void ClearResults()
		{
			readResultsViewModel.ReadResults = "";
		}
	}
}

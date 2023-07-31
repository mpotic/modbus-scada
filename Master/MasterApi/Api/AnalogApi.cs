using Common.DTO;
using Common.Util;
using MasterApi.ViewModel;
using MasterServices.ServiceProviders;
using System;

namespace MasterApi.Api
{
	public class AnalogApi : IAnalogApi
	{
		readonly IAnalogServiceProvider analogService;

		readonly IReadResultsViewModel readResultsViewModel;

		readonly IMessageBoxCallback messageBoxCallback;

		readonly IResponseValuesFormatter formatter = new ResponseValuesFormatter();

		public AnalogApi(IAnalogServiceProvider analogService, IReadResultsViewModel readResultsViewModel, IMessageBoxCallback messageBoxCallback)
		{
			this.analogService = analogService;
			this.readResultsViewModel = readResultsViewModel;
			this.messageBoxCallback = messageBoxCallback;
		}

		public async void ReadInput(IModbusActionParams actionParams)
		{
			IReadAnalogResponse response = await analogService.ReadInput(actionParams);

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);
				return;
			}

			UpdateViewModel(response.Values, actionParams.StartAddress);
		}

		public async void ReadHolding(IModbusActionParams actionParams)
		{
			IReadAnalogResponse response = await analogService.ReadHolding(actionParams);

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);
				return;
			}

			UpdateViewModel(response.Values, actionParams.StartAddress);
		}

		public void WriteHolding(IModbusActionParams actionParams)
		{
			IResponse response = analogService.WriteHolding(actionParams);

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);
			}
		}

		private void UpdateViewModel(Array values, ushort startAddress)
		{
			string formattedResponse = formatter.FormatArray(values, startAddress);
			readResultsViewModel.ReadResults = formattedResponse;
		}
	}
}

using Common.ActionDto;
using Common.ResponseDto;
using Common.Util;
using ModbusApi.ViewModel;
using ModbusServices.ResponseDto.Analog;
using ModbusServices.ServiceProviders;
using System;

namespace ModbusApi.Api
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
			IAnalogReadResponse response = await analogService.ReadInput(actionParams);

			if (!response.IsSuccessfull)
			{
				messageBoxCallback.DisplayError(response.Message);
				return;
			}

			UpdateViewModel(response.Values, actionParams.StartAddress);
		}

		public async void ReadHolding(IModbusActionParams actionParams)
		{
			IAnalogReadResponse response = await analogService.ReadHolding(actionParams);

			if (!response.IsSuccessfull)
			{
				messageBoxCallback.DisplayError(response.Message);
				return;
			}

			UpdateViewModel(response.Values, actionParams.StartAddress);
		}

		public void WriteHolding(IModbusActionParams actionParams)
		{
			IOperationResponse response = analogService.WriteHolding(actionParams);

			if (!response.IsSuccessfull)
			{
				messageBoxCallback.DisplayError(response.Message);
			}
		}

		private void UpdateViewModel(Array values, ushort startAddress)
		{
			string formattedResponse = formatter.FormatArray(values, startAddress);
			readResultsViewModel.ReadResults = formattedResponse;
		}
	}
}

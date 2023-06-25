using Common.ActionDto;
using Common.ResponseDto;
using Common.Util;
using ModbusApi.ViewModel;
using ModbusServices.ResponseDto.Discrete;
using ModbusServices.ServiceProviders;
using System;

namespace ModbusApi.Api
{
	public class DiscreteApi : IDiscreteApi
	{
		readonly IDiscreteServiceProvider discreteService;

		readonly IReadResultsViewModel readResultsViewModel;

		readonly IMessageBoxCallback messageBoxCallback;

		readonly IResponseValuesFormatter formatter = new ResponseValuesFormatter();

		public DiscreteApi(IDiscreteServiceProvider discreteService, IReadResultsViewModel readResultsViewModel, IMessageBoxCallback messageBoxCallback)
		{
			this.discreteService = discreteService;
			this.readResultsViewModel = readResultsViewModel;
			this.messageBoxCallback = messageBoxCallback;
		}

		public void ReadInput(IModbusActionParams actionParams)
		{
			IDiscreteReadResponse response = discreteService.ReadInput(actionParams);

			if (!response.IsSuccessfull)
			{
				messageBoxCallback.DisplayError(response.Message);
				return;
			}

			UpdateViewModel(response.Values, actionParams.StartAddress);
		}

		public void ReadCoil(IModbusActionParams actionParams)
		{
			IDiscreteReadResponse response = discreteService.ReadCoil(actionParams);

			if (!response.IsSuccessfull)
			{
				messageBoxCallback.DisplayError(response.Message);
				return;
			}

			UpdateViewModel(response.Values, actionParams.StartAddress);
		}

		public void WriteCoil(IModbusActionParams actionParams)
		{
			IOperationResponse response = discreteService.WriteCoil(actionParams);

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

using Common.DTO;
using Common.Util;
using MasterApi.ViewModel;
using MasterServices.ServiceProviders;
using System;

namespace MasterApi.Api
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

		public async void ReadInput(IModbusActionParams actionParams)
		{
			IReadDiscreteResponse response = await discreteService.ReadInput(actionParams);

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);
				return;
			}

			UpdateViewModel(response.ByteValues, actionParams.StartAddress);
		}

		public async void ReadCoil(IModbusActionParams actionParams)
		{
			IReadDiscreteResponse response = await discreteService.ReadCoil(actionParams);

			if (!response.IsSuccessful)
			{
				messageBoxCallback.DisplayError(response.ErrorMessage);
				return;
			}

			UpdateViewModel(response.ByteValues, actionParams.StartAddress);
		}

		public void WriteCoil(IModbusActionParams actionParams)
		{
			IResponse response = discreteService.WriteCoil(actionParams);

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

using Common.ActionDto;
using Common.ResponseDto;
using Common.Util;
using ModbusServices.ResponseDto.Discrete;
using System;
using System.Threading.Tasks;

namespace ModbusServices.ServiceProviders
{
	public class DiscreteServiceProvider : IDiscreteServiceProvider
	{
		IService service;

		ByteArrayConverter byteConverter = new ByteArrayConverter();

		internal void SetService(IService service)
		{
			this.service = service;
		}

		public async Task<IDiscreteReadResponse> ReadInput(IModbusActionParams actionParams)
		{
			IDiscreteReadResponse response;

			try
			{
				bool[] boolArray = await service.ReadDiscreteInput(actionParams.SlaveAddress, actionParams.StartAddress, actionParams.NumberOfPoints);
				byte[] byteArray = byteConverter.ConvertToByteArray(boolArray);

				response = new DiscreteReadResponse(true, byteArray);
			}
			catch (Exception e)
			{
				response = new DiscreteReadResponse(false, e.Message);
			}

			return response;
		}

		public async Task<IDiscreteReadResponse> ReadCoil(IModbusActionParams actionParams)
		{
			IDiscreteReadResponse response;

			try
			{
				bool[] boolArray = await service.ReadCoil(actionParams.SlaveAddress, actionParams.StartAddress, actionParams.NumberOfPoints);
				byte[] byteArray = byteConverter.ConvertToByteArray(boolArray);

				response = new DiscreteReadResponse(true, byteArray);
			}
			catch (Exception e)
			{
				response = new DiscreteReadResponse(false, e.Message);
			}

			return response;
		}

		public IOperationResponse WriteCoil(IModbusActionParams actionParams)
		{
			IOperationResponse response;

			try
			{
				bool[] boolArray = byteConverter.ConvertToBoolArray(actionParams.CoilWriteValues);
				service.WriteCoil(actionParams.SlaveAddress, actionParams.StartAddress, boolArray);

				response = new OperationResponse(true);
			}
			catch (Exception e)
			{
				response = new OperationResponse(false, e.Message);
			}

			return response;
		}
	}
}

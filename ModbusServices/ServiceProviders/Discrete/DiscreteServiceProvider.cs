using Common.ActionDto;
using Common.ResponseDto;
using Common.Util;
using ModbusServices.ResponseDto.Discrete;
using System;

namespace ModbusServices.ServiceProviders
{
	public class DiscreteServiceProvider : IDiscreteServiceProvider
	{
		IService service;

		ByteBoolConverter byteConverter = new ByteBoolConverter();

		internal void SetService(IService service)
		{
			this.service = service;
		}

		public IDiscreteReadResponse ReadInput(IModbusActionParams actionParams)
		{
			IDiscreteReadResponse response;

			try
			{
				bool[] boolArray = service.ReadDiscreteInput(actionParams.SlaveAddress, actionParams.StartAddress, actionParams.NumberOfPoints);
				byte[] byteArray = byteConverter.BoolArrayToByteArray(boolArray);

				response = new DiscreteReadResponse(true, byteArray);
			}
			catch (Exception e)
			{
				response = new DiscreteReadResponse(false, e.Message);
			}

			return response;
		}

		public IDiscreteReadResponse ReadCoil(IModbusActionParams actionParams)
		{
			IDiscreteReadResponse response;

			try
			{
				bool[] boolArray = service.ReadCoil(actionParams.SlaveAddress, actionParams.StartAddress, actionParams.NumberOfPoints);
				byte[] byteArray = byteConverter.BoolArrayToByteArray(boolArray);

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
				bool[] boolArray = byteConverter.ByteArrayToBoolArray(actionParams.CoilWriteValues);
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

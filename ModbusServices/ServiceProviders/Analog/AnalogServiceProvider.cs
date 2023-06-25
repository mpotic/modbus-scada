using Common.ActionDto;
using Common.ResponseDto;
using ModbusServices.ResponseDto.Analog;
using System;

namespace ModbusServices.ServiceProviders
{
	public class AnalogServiceProvider : IAnalogServiceProvider
	{
		IService service;

		internal void SetService(IService service)
		{
			this.service = service;
		}

		public IAnalogReadResponse ReadHolding(IModbusActionParams actionParams)
		{
			IAnalogReadResponse response;

			try
			{
				ushort[] result = service.ReadHolding(actionParams.SlaveAddress, actionParams.StartAddress, actionParams.NumberOfPoints);

				response = new AnalogReadResponse(true, result);
			}
			catch (Exception e)
			{
				response = new AnalogReadResponse(false, e.Message);
			}

			return response;
		}

		public IAnalogReadResponse ReadInput(IModbusActionParams actionParams)
		{
			IAnalogReadResponse response;

			try
			{
				ushort[] result = service.ReadAnalogInput(actionParams.SlaveAddress, actionParams.StartAddress, actionParams.NumberOfPoints);

				response = new AnalogReadResponse(true, result);
			}
			catch (Exception e)
			{
				response = new AnalogReadResponse(false, e.Message);
			}

			return response;
		}

		public IOperationResponse WriteHolding(IModbusActionParams actionParams)
		{
			IOperationResponse response;

			try
			{
				service.WriteHolding(actionParams.SlaveAddress, actionParams.StartAddress, actionParams.HoldingWriteValues);

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

using Common.DTO;
using System;
using System.Threading.Tasks;

namespace MasterServices.ServiceProviders
{
	public class AnalogServiceProvider : IAnalogServiceProvider
	{
		IModbusService service;

		internal void SetService(IModbusService service)
		{
			this.service = service;
		}

		public async Task<IReadAnalogResponse> ReadHolding(IModbusActionParams actionParams)
		{
			IReadAnalogResponse response;

			try
			{
				ushort[] result = await service.ReadHolding(actionParams.SlaveAddress, actionParams.StartAddress, actionParams.NumberOfPoints);

				response = new ReadAnalogResponse(result);
			}
			catch (Exception e)
			{
				response = new ReadAnalogResponse(false, e.Message);
			}

			return response;
		}

		public async Task<IReadAnalogResponse> ReadInput(IModbusActionParams actionParams)
		{
			IReadAnalogResponse response;

			try
			{
				ushort[] result = await service.ReadAnalogInput(actionParams.SlaveAddress, actionParams.StartAddress, actionParams.NumberOfPoints);

				response = new ReadAnalogResponse(result);
			}
			catch (Exception e)
			{
				response = new ReadAnalogResponse(false, e.Message);
			}

			return response;
		}

		public IResponse WriteHolding(IModbusActionParams actionParams)
		{
			IResponse response;

			try
			{
				service.WriteHolding(actionParams.SlaveAddress, actionParams.StartAddress, actionParams.HoldingWriteValues);

				response = new Response(true);
			}
			catch (Exception e)
			{
				response = new Response(false, e.Message);
			}

			return response;
		}
	}
}

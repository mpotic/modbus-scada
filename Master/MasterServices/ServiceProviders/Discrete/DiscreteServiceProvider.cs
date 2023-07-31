using Common.DTO;
using Common.Util;
using System;
using System.Threading.Tasks;

namespace MasterServices.ServiceProviders
{
	public class DiscreteServiceProvider : IDiscreteServiceProvider
	{
		IModbusService service;

		ByteArrayConverter byteConverter = new ByteArrayConverter();

		internal void SetService(IModbusService service)
		{
			this.service = service;
		}

		public async Task<IReadDiscreteResponse> ReadInput(IModbusActionParams actionParams)
		{
			IReadDiscreteResponse response;

			try
			{
				bool[] boolArray = await service.ReadDiscreteInput(actionParams.SlaveAddress, actionParams.StartAddress, actionParams.NumberOfPoints);

				response = new ReadDiscreteResponse(boolArray);
			}
			catch (Exception e)
			{
				response = new ReadDiscreteResponse(false, e.Message);
			}

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadCoil(IModbusActionParams actionParams)
		{
			IReadDiscreteResponse response;

			try
			{
				bool[] boolArray = await service.ReadCoil(actionParams.SlaveAddress, actionParams.StartAddress, actionParams.NumberOfPoints);

				response = new ReadDiscreteResponse(boolArray);
			}
			catch (Exception e)
			{
				response = new ReadDiscreteResponse(false, e.Message);
			}

			return response;
		}

		public IResponse WriteCoil(IModbusActionParams actionParams)
		{
			IResponse response;

			try
			{
				bool[] boolArray = byteConverter.ConvertToBoolArray(actionParams.CoilWriteValues);
				service.WriteCoil(actionParams.SlaveAddress, actionParams.StartAddress, boolArray);

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

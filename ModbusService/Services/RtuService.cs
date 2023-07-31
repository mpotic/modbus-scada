using Common.DTO;
using System.Threading.Tasks;

namespace ModbusService
{
	internal class RtuService : IRtuService
	{
		private readonly IConnectionHandle connection;

		public RtuService(IConnectionHandle handle)
		{
			connection = handle;
		}

		public async Task<IReadAnalogResponse> ReadHolding(IReadParams readParams)
		{
			ushort[] values = await connection.ModbusMaster.ReadHoldingRegistersAsync(
				readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);
			IReadAnalogResponse response = new ReadAnalogResponse(values);

			return response;
		}

		public async Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams)
		{
			ushort[] values = await connection.ModbusMaster.ReadInputRegistersAsync(
				readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);
			IReadAnalogResponse response = new ReadAnalogResponse(values);

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams)
		{
			bool[] values = await connection.ModbusMaster.ReadCoilsAsync(
				readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);
			IReadDiscreteResponse response = new ReadDiscreteResponse(values);

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams)
		{
			bool[] values = await connection.ModbusMaster.ReadInputsAsync(
				readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);
			IReadDiscreteResponse response = new ReadDiscreteResponse(values);

			return response;
		}

		public IResponse WriteHolding(IWriteHoldingParams writeParams)
		{
			connection.ModbusMaster.WriteMultipleRegisters(
				writeParams.SlaveAddress, writeParams.StartAddress, writeParams.WriteValues);
			IResponse response = new Response(true);

			return response;
		}

		public IResponse WriteCoil(IWriteCoilParams writeParams)
		{
			connection.ModbusMaster.WriteMultipleCoils(
				writeParams.SlaveAddress, writeParams.StartAddress, writeParams.WriteValues);
			IResponse response = new Response(true);

			return response;
		}
	}
}

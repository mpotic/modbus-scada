using Common.Connection;
using Common.DTO;
using Common.Util;
using Common.Enums;
using System.Threading.Tasks;
using TcpService;

namespace MasterServices
{
	internal class TcpServiceProvider : ITcpServiceProvider
	{
		ICommunicationApi tcpServiceCommunicationApi;

		IConnectionApi tcpServiceConnectionApi;

		ITcpSerializer tcpSerializer = new TcpSerializer();

		IByteArrayConverter converter = new ByteArrayConverter();

		public async Task<IResponse> Connenct(IConnectionParams connectionParams)
		{
			return await tcpServiceConnectionApi.Connect(connectionParams);
		}

		public IResponse Disconnect()
		{
			return tcpServiceConnectionApi.Disconnect();
		}

		public async Task<IReadAnalogResponse> ReadHolding(IReadParams readParams)
		{
			tcpSerializer.InitMessage();
			tcpSerializer.AddHeader(SenderCode.Master, FunctionCode.ReadHolding);
			tcpSerializer.AddBody(readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);
			
			tcpServiceCommunicationApi.Send(tcpSerializer.Message);
			ITcpReceiveResponse tcpResponse = await tcpServiceCommunicationApi.Receive();

			ushort[] values = converter.ConvertToUshortArray(tcpResponse.Payload);
			IReadAnalogResponse response = new ReadAnalogResponse(values);

			return response;
		}

		public async Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams)
		{
			tcpSerializer.InitMessage();
			tcpSerializer.AddHeader(SenderCode.Master, FunctionCode.ReadAnalogInputs);
			tcpSerializer.AddBody(readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);

			tcpServiceCommunicationApi.Send(tcpSerializer.Message);
			ITcpReceiveResponse tcpResponse = await tcpServiceCommunicationApi.Receive();

			ushort[] values = converter.ConvertToUshortArray(tcpResponse.Payload);
			IReadAnalogResponse response = new ReadAnalogResponse(values);

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams)
		{
			tcpSerializer.InitMessage();
			tcpSerializer.AddHeader(SenderCode.Master, FunctionCode.ReadCoils);
			tcpSerializer.AddBody(readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);

			tcpServiceCommunicationApi.Send(tcpSerializer.Message);
			ITcpReceiveResponse tcpResponse = await tcpServiceCommunicationApi.Receive();
			IReadDiscreteResponse response = new ReadDiscreteResponse(tcpResponse.Payload);

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams)
		{
			tcpSerializer.InitMessage();
			tcpSerializer.AddHeader(SenderCode.Master, FunctionCode.ReadDiscreteInputs);
			tcpSerializer.AddBody(readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);

			tcpServiceCommunicationApi.Send(tcpSerializer.Message);
			ITcpReceiveResponse tcpResponse = await tcpServiceCommunicationApi.Receive();
			IReadDiscreteResponse response = new ReadDiscreteResponse(tcpResponse.Payload);

			return response;
		}

		public IResponse WriteHolding(IWriteHoldingParams writeParams)
		{
			byte[] values = converter.ConvertToByteArray(writeParams.WriteValues);
			IResponse response = tcpServiceCommunicationApi.Send(values);

			return response;
		}

		public IResponse WriteCoil(IWriteCoilParams writeParams)
		{
			byte[] values = converter.ConvertToByteArray(writeParams.WriteValues);
			IResponse response = tcpServiceCommunicationApi.Send(values);

			return response;
		}
	}
}

using Common.Connection;
using Common.DTO;
using Common.Util;
using Common.Enums;
using System.Threading.Tasks;
using TcpService;
using Common.Callback;

namespace MasterServices
{
	internal sealed class TcpServiceProvider : ITcpServiceProvider
	{
		ICommunicationApi communicationService;

		IConnectionApi connectionService;

		ITcpSerializer serializer = new TcpSerializer();

		IByteArrayConverter converter = new ByteArrayConverter();

		public TcpServiceProvider(ITcpServiceHandler tcpServiceHandler)
		{
			communicationService = tcpServiceHandler.CommunicationApi;
			connectionService = tcpServiceHandler.ConnectionApi;
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			return await connectionService.Connect(connectionParams);
		}

		public IResponse Disconnect()
		{
			return connectionService.Disconnect();
		}

		public async Task<IReadAnalogResponse> ReadHolding(IReadParams readParams)
		{
			serializer.InitMessage();
			serializer.AddHeader(SenderCode.Master, FunctionCode.ReadHolding);
			serializer.AddBody(readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);
			
			communicationService.Send(serializer.Message);
			ITcpReceiveResponse tcpResponse = await communicationService.Receive();

			ushort[] values = converter.ConvertToUshortArray(tcpResponse.Payload);
			IReadAnalogResponse response = new ReadAnalogResponse(values);

			return response;
		}

		public async Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams)
		{
			serializer.InitMessage();
			serializer.AddHeader(SenderCode.Master, FunctionCode.ReadAnalogInputs);
			serializer.AddBody(readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);

			communicationService.Send(serializer.Message);
			ITcpReceiveResponse tcpResponse = await communicationService.Receive();

			ushort[] values = converter.ConvertToUshortArray(tcpResponse.Payload);
			IReadAnalogResponse response = new ReadAnalogResponse(values);

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams)
		{
			serializer.InitMessage();
			serializer.AddHeader(SenderCode.Master, FunctionCode.ReadCoils);
			serializer.AddBody(readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);

			communicationService.Send(serializer.Message);
			ITcpReceiveResponse tcpResponse = await communicationService.Receive();
			IReadDiscreteResponse response = new ReadDiscreteResponse(tcpResponse.Payload);

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams)
		{
			serializer.InitMessage();
			serializer.AddHeader(SenderCode.Master, FunctionCode.ReadDiscreteInputs);
			serializer.AddBody(readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);

			communicationService.Send(serializer.Message);
			ITcpReceiveResponse tcpResponse = await communicationService.Receive();
			IReadDiscreteResponse response = new ReadDiscreteResponse(tcpResponse.Payload);

			return response;
		}

		public IResponse WriteHolding(IWriteHoldingParams writeParams)
		{
			byte[] values = converter.ConvertToByteArray(writeParams.WriteValues);
			IResponse response = communicationService.Send(values);

			return response;
		}

		public IResponse WriteCoil(IWriteCoilParams writeParams)
		{
			byte[] values = converter.ConvertToByteArray(writeParams.WriteValues);
			IResponse response = communicationService.Send(values);

			return response;
		}

		public void RegisterCallack(IConnectionStatusCallback callback)
		{
			connectionService.RegisterCallack(callback);
		}
	}
}

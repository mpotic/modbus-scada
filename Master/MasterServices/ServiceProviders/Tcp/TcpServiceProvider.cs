using Common.Connection;
using Common.DTO;
using Common.Enums;
using System.Threading.Tasks;
using TcpService;
using Common.Callback;
using System;

namespace MasterServices
{
	internal sealed class TcpServiceProvider : ITcpServiceProvider
	{
		ICommunicationApi communicationService;

		IConnectionApi connectionService;

		ITcpSerializer serializer = new TcpSerializer();

		public TcpServiceProvider(ITcpServiceHandler tcpServiceHandler)
		{
			communicationService = tcpServiceHandler.CommunicationApi;
			connectionService = tcpServiceHandler.ConnectionApi;
		}

		public async Task<IResponse> Connect(IConnectionParams connectionParams)
		{
			try
			{
				return await connectionService.Connect(connectionParams);
			}
			catch(Exception e)
			{
				return new Response(false, "Connect request failed! " + e.Message);
			}
		}

		public IResponse Disconnect()
		{
			try
			{
				return connectionService.Disconnect();
			}
			catch (Exception e)
			{
				return new Response(false, "Disconnect request failed! " + e.Message);
			}
		}

		public async Task<IReadAnalogResponse> ReadHolding(IReadParams readParams)
		{
			serializer.InitMessage();
			serializer.AddHeader(SenderCode.Master, FunctionCode.ReadHolding);
			serializer.AddBody(readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);
			serializer.AddSizeToHeader();

			IReadAnalogResponse response;
			try
			{
				IResponse sendResponse = communicationService.Send(serializer.Message);
				if (!sendResponse.IsSuccessful)
				{
					return new ReadAnalogResponse(sendResponse.IsSuccessful,
						"Read holding request failed! " + sendResponse.ErrorMessage);
				}

				ITcpReceiveResponse receiveResponse = await communicationService.ReceiveWithTimeout();
				if (!receiveResponse.IsSuccessful)
				{
					return new ReadAnalogResponse(receiveResponse.IsSuccessful,
						"Read holding request failed! " + receiveResponse.ErrorMessage);
				}

				serializer.InitMessage(receiveResponse.Payload);
				response = new ReadAnalogResponse(serializer.ReadAnalogReadValuesFromBody());
			}
			catch(Exception e)
			{
				response = new ReadAnalogResponse(false, "Read holding request failed! " + e.Message);
			}

			return response;
		}

		public async Task<IReadAnalogResponse> ReadAnalogInput(IReadParams readParams)
		{
			serializer.InitMessage();
			serializer.AddHeader(SenderCode.Master, FunctionCode.ReadAnalogInputs);
			serializer.AddBody(readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);
			serializer.AddSizeToHeader();

			IReadAnalogResponse response;
			try
			{
				IResponse sendResponse = communicationService.Send(serializer.Message);
				if (!sendResponse.IsSuccessful)
				{
					return new ReadAnalogResponse(sendResponse.IsSuccessful,
						"Read analog input request failed! " + sendResponse.ErrorMessage);
				}

				ITcpReceiveResponse receiveResponse = await communicationService.ReceiveWithTimeout();
				if (!receiveResponse.IsSuccessful)
				{
					return new ReadAnalogResponse(receiveResponse.IsSuccessful,
						"Read analog input request failed! " + receiveResponse.ErrorMessage);
				}

				serializer.InitMessage(receiveResponse.Payload);
				response = new ReadAnalogResponse(serializer.ReadAnalogReadValuesFromBody());
			}
			catch (Exception e)
			{
				response = new ReadAnalogResponse(false, "Read analog input request failed! " +  e.Message);
			}

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadCoil(IReadParams readParams)
		{
			serializer.InitMessage();
			serializer.AddHeader(SenderCode.Master, FunctionCode.ReadCoils);
			serializer.AddBody(readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);
			serializer.AddSizeToHeader();

			IReadDiscreteResponse response;
			try
			{
				IResponse sendResponse = communicationService.Send(serializer.Message);
				if (!sendResponse.IsSuccessful)
				{
					return new ReadDiscreteResponse(sendResponse.IsSuccessful,
						"Read coils request failed! " + sendResponse.ErrorMessage);
				}

				ITcpReceiveResponse receiveResponse = await communicationService.ReceiveWithTimeout();
				if (!receiveResponse.IsSuccessful)
				{
					return new ReadDiscreteResponse(receiveResponse.IsSuccessful,
						"Read coils request failed! " + receiveResponse.ErrorMessage);
				}

				serializer.InitMessage(receiveResponse.Payload);
				response = new ReadDiscreteResponse(serializer.ReadDiscreteValuesFromBody());
			}
			catch (Exception e)
			{
				response = new ReadDiscreteResponse(false, "Read coils request failed! " + e.Message);
			}

			return response;
		}

		public async Task<IReadDiscreteResponse> ReadDiscreteInput(IReadParams readParams)
		{
			serializer.InitMessage();
			serializer.AddHeader(SenderCode.Master, FunctionCode.ReadDiscreteInputs);
			serializer.AddBody(readParams.SlaveAddress, readParams.StartAddress, readParams.NumberOfPoints);
			serializer.AddSizeToHeader();

			IReadDiscreteResponse response;
			try
			{
				IResponse sendResponse = communicationService.Send(serializer.Message);
				if (!sendResponse.IsSuccessful)
				{
					return new ReadDiscreteResponse(sendResponse.IsSuccessful,
						"Read discrete input request failed! " + sendResponse.ErrorMessage);
				}

				ITcpReceiveResponse receiveResponse = await communicationService.ReceiveWithTimeout();
				if (!receiveResponse.IsSuccessful)
				{
					return new ReadDiscreteResponse(receiveResponse.IsSuccessful,
						"Read discrete input request failed! " + receiveResponse.ErrorMessage);
				}

				serializer.InitMessage(receiveResponse.Payload);
				response = new ReadDiscreteResponse(serializer.ReadDiscreteValuesFromBody());
			}
			catch (Exception e)
			{
				response = new ReadDiscreteResponse(false, "Read discrete input request failed! " + e.Message);
			}

			return response;
		}

		public IResponse WriteHolding(IWriteHoldingParams writeParams)
		{
			serializer.InitMessage();
			serializer.AddHeader(SenderCode.Master, FunctionCode.WriteHolding);
			serializer.AddBody(writeParams.SlaveAddress, writeParams.StartAddress, writeParams.WriteValues);
			serializer.AddSizeToHeader();

			IResponse response;
			try
			{
				response = communicationService.Send(serializer.Message);
			}
			catch (Exception e)
			{
				response = new Response(false, "Write holding request failed! " + e.Message);
			}

			return response;
		}

		public IResponse WriteCoil(IWriteCoilParams writeParams)
		{
			serializer.InitMessage();
			serializer.AddHeader(SenderCode.Master, FunctionCode.WriteCoils);
			serializer.AddBody(writeParams.SlaveAddress, writeParams.StartAddress, writeParams.ByteWriteValues);
			serializer.AddSizeToHeader();

			IResponse response;
			try
			{
				response = communicationService.Send(serializer.Message);
			}
			catch (Exception e)
			{
				response = new Response(false, "Write coils request failed! " + e.Message);
			}

			return response;
		}

		public void RegisterCallack(IConnectionStatusCallback callback)
		{
			connectionService.RegisterCallack(callback);
		}
	}
}

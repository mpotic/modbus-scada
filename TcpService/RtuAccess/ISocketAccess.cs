﻿using Common.DTO;
using Common.Enums;
using System.Threading.Tasks;

namespace TcpService
{
	internal interface ISocketAccess
	{
		void TransitionState(IState state, ConnectionStatusCode statusCode);

		Task<IResponse> Connect(IConnectionParams connectionParams);

		IResponse Disconnect();

		Task<IResponse> Listen(IConnectionParams connectionParams);

		Task<ITcpReceiveResponse> Receive();

		Task<ITcpReceiveResponse> ReceiveWithTimeout();

		IResponse Send(byte[] message);

		IResponse ClearReceiveBuffer();
	}
}

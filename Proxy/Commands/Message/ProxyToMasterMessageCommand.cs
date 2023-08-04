using Common.Connection;
using Common.Enums;
using Common.Util;
using Proxy.Connections;
using System;
using System.Collections.Generic;

namespace Proxy.Commands
{
	class ProxyToMasterMessageCommand : IMessageCommand
	{
		private ITcpSerializer serializer;

		private ITcpConnection connection;

		IByteArrayConverter converter = new ByteArrayConverter();

		private Dictionary<FunctionCode, Func<byte[]>> actions;

		public void SetParams(IConnection connection, ITcpSerializer serializer)
		{
			actions = new Dictionary<FunctionCode, Func<byte[]>>
			{
				{ FunctionCode.ReadCoils, GetDiscrete },
				{ FunctionCode.ReadDiscreteInputs, GetDiscrete },
				{ FunctionCode.ReadHolding, GetAnalog },
				{ FunctionCode.ReadAnalogInputs, GetAnalog }
			};

			this.serializer = serializer;
			this.connection = (ITcpConnection)connection;
		}

		public void Execute()
		{
			FunctionCode functionCode = serializer.ReadFunctionCodeFromHeader();
			byte[] values = actions[functionCode].Invoke();
			connection.Communication.Send(values);
		}

		private byte[] GetDiscrete()
		{
			bool[] boolValues = serializer.ReadDiscreteReadValuesFromBody();

			return converter.ConvertToByteArray(boolValues);
		}

		private byte[] GetAnalog()
		{
			ushort[] ushortValues = serializer.ReadAnalogReadValuesFromBody();

			return converter.ConvertToByteArray(ushortValues);
		}
	}
}

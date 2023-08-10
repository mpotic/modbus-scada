using Common.Connection;
using Common.Enums;
using Common.Util;
using Proxy.Connections;

namespace Proxy.Commands
{
	class ProxyToMasterMessageCommand : IMessageCommand
	{
		private ITcpSerializer serializer;

		private ITcpConnection connection;

		IByteArrayConverter converter = new ByteArrayConverter();

		public void SetParams(IConnection connection, ITcpSerializer serializer)
		{
			this.serializer = serializer;
			this.connection = (ITcpConnection)connection;
		}

		public void Execute()
		{
			FunctionCode functionCode = serializer.ReadFunctionCodeFromHeader();
			SenderCode senderCode = SenderCode.Master;
			serializer.ReplaceHeader(senderCode, functionCode);
			serializer.AddSizeToHeader();
			connection.Communication.Send(serializer.Message);
		}
	}
}

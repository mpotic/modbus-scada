using Common.Connection;
using Common.Enums;
using Proxy.Connections;
using Proxy.Security;

namespace Proxy.Commands
{
	class ProxyToMasterMessageCommand : IMessageCommand
	{
		private ITcpSerializer serializer;

		private ITcpConnection connection;

		private byte[] message;

		private readonly ISecurityHandler security;

		internal ProxyToMasterMessageCommand(ISecurityHandler security)
		{
			this.security = security;
		}

		public void SetParams(IConnection connection, byte[] message)
		{
			this.connection = (ITcpConnection)connection;
			this.message = message;
		}

		public void Execute()
		{
			ITcpSerializer serializer = new TcpSerializer();
			serializer.InitMessage(message);
			FunctionCode functionCode = serializer.ReadFunctionCodeFromHeader();
			SenderCode senderCode = SenderCode.Master;
			serializer.ReplaceHeader(senderCode, functionCode);
			serializer.AddSizeToHeader();

			connection.Communication.Send(serializer.Message);
		}
	}
}

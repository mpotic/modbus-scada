using Common.Connection;
using Common.Enums;
using Proxy.Connections;
using Proxy.Security;

namespace Proxy.Commands
{
    internal class MasterMessageCommand : IMessageCommand
    {
        private byte[] message;

        private ITcpConnection connection;

        private readonly ISecurityHandler security;

        internal MasterMessageCommand(ISecurityHandler security)
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
            FunctionCode requestCode = serializer.ReadFunctionCodeFromHeader();
            serializer.ReplaceHeader(SenderCode.ProxyToSlave, requestCode);
            serializer.AddSizeToHeader();

            byte[] securedMessage = security.Secure(serializer.Message);
            connection.Communication.Send(securedMessage);
        }
    }
}

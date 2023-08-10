using Common.Connection;
using Common.Enums;
using Proxy.Connections;

namespace Proxy.Commands
{
    internal class MasterMessageCommand : IMessageCommand
    {
        private ITcpSerializer serializer;

        private ITcpConnection connection;

        public void SetParams(IConnection connection, ITcpSerializer serializer)
        {
            this.connection = (ITcpConnection)connection;
            this.serializer = serializer;
        }

        public void Execute()
        {
            FunctionCode requestCode = serializer.ReadFunctionCodeFromHeader();
            serializer.ReplaceHeader(SenderCode.ProxyToSlave, requestCode);
            serializer.AddSizeToHeader();
            connection.Communication.Send(serializer.Message);
        }
    }
}

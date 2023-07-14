using Common.Connection;
using Common.Enums;

namespace Proxy.MessageHandling
{
    internal class MasterMessageHandler : IMessageHandler
    {
        private readonly ITcpSerializer serializer;

        private readonly ITcpSocketHandler proxySocket;

        public MasterMessageHandler(ITcpSerializer serializer, ITcpSocketHandler socket)
        {
            this.serializer = serializer;
            this.proxySocket = socket;
        }

        public void Process()
        {
            FunctionCode requestCode = serializer.ReadRequestCodeFromHeader();
            serializer.ReplaceHeader(SenderCode.ProxyToSlave, requestCode);
            proxySocket.Send(serializer.Message);
        }
    }
}

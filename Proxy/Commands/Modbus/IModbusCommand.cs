using Common.Connection;
using Proxy.Connections;

namespace Proxy.Commands
{
	internal interface IModbusCommand
    {
        void SetParams(IConnection connection, ITcpSerializer serializer);

        void Execute();
    }
}

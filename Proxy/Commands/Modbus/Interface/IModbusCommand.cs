using Common.Connection;

namespace Proxy.Commands
{
	internal interface IModbusCommand
    {
        void SetParams(ITcpSerializer serializer);
    }
}

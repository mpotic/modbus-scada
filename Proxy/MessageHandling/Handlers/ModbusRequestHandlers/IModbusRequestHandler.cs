using NModbus;

namespace Proxy.MessageHandling.Handlers.ModbusRequestHandlers
{
    internal interface IModbusRequestHandler
    {
        void Process(IModbusMaster slave);
    }
}

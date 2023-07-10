using Common.Connection;
using NModbus;

namespace Proxy.MessageHandling.Handlers.ModbusRequestHandlers
{
    internal class WriteHoldingHandler : IModbusRequestHandler
    {
        private readonly ITcpSerializer serializer;

        public WriteHoldingHandler(ITcpSerializer serialzier)
        {
            this.serializer = serialzier;
        }

        public async void Process(IModbusMaster slave)
        {
            byte slaveAddress = serializer.ReadSlaveAddressFromBody();
            ushort startAddress = serializer.ReadStartAddressFromBody();
            ushort[] writeValues = serializer.ReadHoldingWriteValuesFromBody();
            await slave.WriteMultipleRegistersAsync(slaveAddress, startAddress, writeValues);
        }
    }
}

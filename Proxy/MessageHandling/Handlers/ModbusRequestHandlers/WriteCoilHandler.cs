using Common.Connection;
using NModbus;

namespace Proxy.MessageHandling.Handlers.ModbusRequestHandlers
{
    internal class WriteCoilHandler : IModbusRequestHandler
    {
        private readonly ITcpSerializer serializer;

        public WriteCoilHandler(ITcpSerializer serialzier)
        {
            this.serializer = serialzier;
        }

        public async void Process(IModbusMaster slave)
        {
            byte slaveAddress = serializer.ReadSlaveAddressFromBody();
            ushort startAddress = serializer.ReadStartAddressFromBody();
            bool[] writeValues = serializer.ReadCoilWriteValuesFromBody();
            await slave.WriteMultipleCoilsAsync(slaveAddress, startAddress, writeValues);
        }
    }
}

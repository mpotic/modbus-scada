using Common.Connection;
using Common.Enums;
using NModbus;

namespace Proxy.MessageHandling.Handlers.ModbusRequestHandlers
{
    internal class ReadAnalogInputHandler : IModbusRequestHandler
    {
        private readonly ITcpSerializer serializer;

        private readonly ITcpSocketHandler proxySocket;

        public ReadAnalogInputHandler(ITcpSerializer serialzier, ITcpSocketHandler socket)
        {
            this.serializer = serialzier;
            this.proxySocket = socket;
        }

        public async void Process(IModbusMaster slave)
        {
            byte slaveAddress = serializer.ReadSlaveAddressFromBody();
            ushort startAddress = serializer.ReadStartAddressFromBody();
            ushort numberOfPoints = serializer.ReadNumberOfPointsFromBody();
            ushort[] values = await slave.ReadInputRegistersAsync(slaveAddress, startAddress, numberOfPoints);

            SendResponse(values);
        }

        private void SendResponse(ushort[] values)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.ProxyToMaster, FunctionCode.ReadAnalogInputs);
            serializer.AddBody(values);
            proxySocket.Send(serializer.Message);
        }
    }
}

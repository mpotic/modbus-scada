using Common.Connection;
using Common.Enums;
using NModbus;

namespace Proxy.MessageHandling.Handlers.ModbusRequestHandlers
{
    internal class ReadDiscreteInputHandler : IModbusRequestHandler
    {
        private readonly ITcpSerializer serializer;

        private readonly ITcpSocketHandler proxySocket;

        public ReadDiscreteInputHandler(ITcpSerializer serializer, ITcpSocketHandler socket)
        {
            this.serializer = serializer;
            this.proxySocket = socket;
        }

        public async void Process(IModbusMaster slave)
        {
            byte slaveAddress = serializer.ReadSlaveAddressFromBody();
            ushort startAddress = serializer.ReadStartAddressFromBody();
            ushort numberOfPoints = serializer.ReadNumberOfPointsFromBody();
            bool[] values = await slave.ReadInputsAsync(slaveAddress, startAddress, numberOfPoints);

            SendResponse(values);
        }

        private void SendResponse(bool[] values)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.ProxyToMaster, ModbusRequestCode.ReadDiscreteInput);
            serializer.AddBody(values);
            proxySocket.Send(serializer.Message);
        }
    }
}

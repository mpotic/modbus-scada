using Common.Connection;
using Common.Enums;
using NModbus;

namespace Proxy.MessageHandling.Handlers.ModbusRequestHandlers
{
    internal class ReadCoilHandler : IModbusRequestHandler
    {
        private readonly ITcpSerializer serializer;

        private readonly ITcpSocketHandler proxySocket;

        public ReadCoilHandler(ITcpSerializer serialzier, ITcpSocketHandler socket)
        {
            this.serializer = serialzier;
            this.proxySocket = socket;
        }

        public async void Process(IModbusMaster slave)
        {
            byte slaveAddress = serializer.ReadSlaveAddressFromBody();
            ushort startAddress = serializer.ReadStartAddressFromBody();
            ushort numberOfPoints = serializer.ReadNumberOfPointsFromBody();
            bool[] values = await slave.ReadCoilsAsync(slaveAddress, startAddress, numberOfPoints);

            SendResponse(values);
        }

        private void SendResponse(bool[] values)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.ProxyToMaster, ModbusRequestCode.ReadCoil);
            serializer.AddBody(values);
            proxySocket.Send(serializer.Message);
        }
    }
}

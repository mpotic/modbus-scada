using Common.Connection;
using Common.DTO;
using Common.Enums;
using Proxy.Connections;

namespace Proxy.Commands
{
	internal class ReadAnalogInputCommand : IModbusCommand
    {
        private ITcpSerializer serializer;

        private ITcpConnection proxy;

        private readonly IModbusConnection slave;

        public ReadAnalogInputCommand(IModbusConnection slave)
        {
            this.slave = slave;
        }

        public void SetParams(IConnection proxy, ITcpSerializer serializer)
        {
            this.proxy = (ITcpConnection)proxy;
            this.serializer = serializer;
        }

        public async void Execute()
        {
            byte slaveAddress = serializer.ReadSlaveAddressFromBody();
            ushort startAddress = serializer.ReadStartAddressFromBody();
            ushort numberOfPoints = serializer.ReadNumberOfPointsFromBody();
            IReadParams readParams = new ReadParams(slaveAddress, startAddress, numberOfPoints);
            IReadAnalogResponse response = await slave.Rtu.ReadAnalogInput(readParams);

            SendResponse(response.Values);
        }

        private void SendResponse(ushort[] values)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.ProxyToMaster, FunctionCode.ReadAnalogInputs);
            serializer.AddBody(values);
            proxy.Communication.Send(serializer.Message);
        }
	}
}

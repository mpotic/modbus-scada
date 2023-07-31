using Common.Connection;
using Common.Enums;
using MasterServices.Connection;
using System.Threading.Tasks;

namespace MasterServices.ServiceProviders
{
	internal class TcpProtocolService : ITcpProtocolService
	{
		private readonly IStandardConnection connection;

		private readonly ITcpSerializer serializer = new TcpSerializer();

		public TcpProtocolService(IStandardConnection connection)
		{
			this.connection = connection;
		}

		public async Task<ushort[]> ReadAnalogInput(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.Master, FunctionCode.ReadAnalogInputs);
            serializer.AddBody(slaveAddress, startAddress, numberOfPoints);
			connection.Connection.Send(serializer.Message);

			byte[] response = await connection.Connection.ReceiveAsync();
            serializer.InitMessage(response);
            ushort[] retVal = serializer.ReadAnalogReadValuesFromBody();

			return retVal;
        }

        public async Task<ushort[]> ReadHolding(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.Master, FunctionCode.ReadHolding);
            serializer.AddBody(slaveAddress, startAddress, numberOfPoints);
            connection.Connection.Send(serializer.Message);

            byte[] response = await connection.Connection.ReceiveAsync();
            serializer.InitMessage(response);
            ushort[] retVal = serializer.ReadAnalogReadValuesFromBody();

            return retVal;
        }

        public async Task<bool[]> ReadCoil(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {

            serializer.InitMessage();
            serializer.AddHeader(SenderCode.Master, FunctionCode.ReadCoils);
            serializer.AddBody(slaveAddress, startAddress, numberOfPoints);
            connection.Connection.Send(serializer.Message);

            byte[] response = await connection.Connection.ReceiveAsync();
            serializer.InitMessage(response);
            bool[] retVal = serializer.ReadDiscreteReadValuesFromBody();

            return retVal;
        }

        public async Task<bool[]> ReadDiscreteInput(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.Master, FunctionCode.ReadDiscreteInputs);
            serializer.AddBody(slaveAddress, startAddress, numberOfPoints);
            connection.Connection.Send(serializer.Message);

            byte[] response = await connection.Connection.ReceiveAsync();
            serializer.InitMessage(response);
            bool[] retVal = serializer.ReadDiscreteReadValuesFromBody();

            return retVal;
        }

		public void WriteCoil(byte slaveAddress, ushort startAddress, bool[] writeValues)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.Master, FunctionCode.WriteCoils);
            serializer.AddBody(slaveAddress, startAddress, writeValues);
            connection.Connection.Send(serializer.Message);
        }

		public void WriteHolding(byte slaveAddress, ushort startAddress, ushort[] writeValues)
        {
            serializer.InitMessage();
            serializer.AddHeader(SenderCode.Master, FunctionCode.WriteHolding);
            serializer.AddBody(slaveAddress, startAddress, writeValues);
            connection.Connection.Send(serializer.Message);
        }
    }
}

using Common.Enums;
using Common.Util;
using System;
using System.Linq;
using System.Text;

namespace Common.Connection
{
    public class TcpSerializer : ITcpSerializer
    {
        public const string separator = "//";

        public const string innerSeparator = ";";

        private readonly IByteArrayConverter byteArrayConverter = new ByteArrayConverter();

        public byte[] Message { get; private set; }

        public void InitMessage()
        {
            Message = Encoding.UTF8.GetBytes(separator);
        }

        public void InitMessage(byte[] message)
        {
            Message = message;
        }

        public void AddHeader(SenderCode senderCode, ModbusRequestCode requestCode)
        {
            string header = senderCode.ToString() + ";" + requestCode.ToString();
            byte[] headerBytes = Encoding.UTF8.GetBytes(header);
            Message = headerBytes.Concat(Message).ToArray();
        }

        public void ReplaceHeader(SenderCode senderCode, ModbusRequestCode requestCode)
        {
            string header = senderCode.ToString() + ";" + requestCode.ToString() + separator;
            byte[] headerBytes = Encoding.UTF8.GetBytes(header);
            string messageString = Encoding.UTF8.GetString(Message);
            string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
            byte[] messageBody = Encoding.UTF8.GetBytes(messageParts[1]);

            Message = headerBytes.Concat(messageBody).ToArray();
        }

        public void AddBody(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            string body = slaveAddress + ";" + startAddress + ";" + numberOfPoints;
            byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
            Message = Message.Concat(bodyBytes).ToArray();
        }

        public void AddBody(byte slaveAddress, ushort startAddress, ushort[] writeValues)
        {
            string body = slaveAddress + ";" + startAddress + ";" + string.Join(",", writeValues);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
            Message = Message.Concat(bodyBytes).ToArray();
        }

        public void AddBody(byte slaveAddress, ushort startAddress, bool[] writeValues)
        {
            string body = slaveAddress + ";" + startAddress + ";" + string.Join(",", writeValues);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
            Message = Message.Concat(bodyBytes).ToArray();
        }

        public void AddBody(ushort[] values)
        {
            string body = string.Join(",", values);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
            Message = Message.Concat(bodyBytes).ToArray();
        }

        public void AddBody(bool[] values)
        {
            string body = string.Join(",", values);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
            Message = Message.Concat(bodyBytes).ToArray();
        }

        public SenderCode ReadSenderCodeFromHeader()
        {
            string messageString = Encoding.UTF8.GetString(Message);
            string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
            string[] headerParts = messageParts[0].Split(new[] { innerSeparator }, StringSplitOptions.None);

            if (headerParts.Length < 2 || !Enum.TryParse(headerParts[0], out SenderCode senderCode))
            {
                return SenderCode.Unknown;
            }

            return senderCode;
        }

        public ModbusRequestCode ReadRequestCodeFromHeader()
        {
            string messageString = Encoding.UTF8.GetString(Message);
            string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
            string[] headerParts = messageParts[0].Split(new[] { innerSeparator }, StringSplitOptions.None);

            if (headerParts.Length < 2)
            {
                throw new Exception("Incorrect message formatting: ModbusRequestCode not found.");
            }

            if (!Enum.TryParse(headerParts[1], out ModbusRequestCode requestCode))
            {
                throw new Exception("Incorrect message formatting: Invalid ModbusRequestCode.");
            }

            return requestCode;
        }

        public byte ReadSlaveAddressFromBody()
        {
            string messageString = Encoding.UTF8.GetString(Message);
            string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
            string[] bodyParts = messageParts[1].Split(new[] { innerSeparator }, StringSplitOptions.None);
            
            if (bodyParts.Length < 3)
            {
                throw new Exception("Incorrect message formatting: SlaveAddress not found.");
            }

            if (!byte.TryParse(bodyParts[0], out byte slaveAddress))
            {
                throw new Exception("Incorrect message formatting: Invalid SlaveAddress.");
            }

            return slaveAddress;
        }

        public ushort ReadStartAddressFromBody()
        {
            string messageString = Encoding.UTF8.GetString(Message);
            string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
            string[] bodyParts = messageParts[1].Split(new[] { innerSeparator }, StringSplitOptions.None);

            if (bodyParts.Length < 3)
            {
                throw new Exception("Incorrect message formatting: StartAddress not found.");
            }

            if (!ushort.TryParse(bodyParts[1], out ushort startAddress))
            {
                throw new Exception("Incorrect message formatting: Invalid StartAddress.");
            }

            return startAddress;
        }

        public ushort ReadNumberOfPointsFromBody()
        {
            string messageString = Encoding.UTF8.GetString(Message);
            string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
            string[] bodyParts = messageParts[1].Split(new[] { innerSeparator }, StringSplitOptions.None);

            if (bodyParts.Length < 3)
            {
                throw new Exception("Incorrect message formatting: NumberOfPoints not found.");
            }

            if (!ushort.TryParse(bodyParts[2], out ushort numberOfPoints))
            {
                throw new Exception("Incorrect message formatting: Invalid NumberOfPoints.");
            }

            return numberOfPoints;
        }

        public ushort[] ReadHoldingWriteValuesFromBody()
        {
            string messageString = Encoding.UTF8.GetString(Message);
            string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
            string[] bodyParts = messageParts[1].Split(new[] { innerSeparator }, StringSplitOptions.None);

            if (bodyParts.Length < 3)
            {
                throw new Exception("Incorrect message formatting: WriteValues not found.");
            }

            string[] values = bodyParts[2].Split(',');

            ushort[] writeValues = new ushort[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (!ushort.TryParse(values[i], out writeValues[i]))
                {
                    throw new Exception("Incorrect message formatting: Invalid WriteValues.");
                }
            }

            return writeValues;
        }

        public bool[] ReadCoilWriteValuesFromBody()
        {
            string messageString = Encoding.UTF8.GetString(Message);
            string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
            string[] bodyParts = messageParts[1].Split(new[] { innerSeparator }, StringSplitOptions.None);

            if (bodyParts.Length < 3)
            {
                throw new Exception("Incorrect message formatting: BoolValues not found.");
            }

            string[] values = bodyParts[2].Split(',');

            bool[] writeValues = new bool[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (!bool.TryParse(values[i], out writeValues[i]))
                {
                    throw new Exception("Incorrect message formatting: Invalid BoolValues.");
                }
            }

            return writeValues;
        }

        public ushort[] ReadAnalogReadValuesFromBody()
        {
            string messageString = Encoding.UTF8.GetString(Message);
            string body = messageString.Split(new[] { separator }, StringSplitOptions.None)[1];

            string[] values = body.Split(',');

            ushort[] readValues = new ushort[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (!ushort.TryParse(values[i], out readValues[i]))
                {
                    throw new Exception("Incorrect message formatting: Invalid ReadValues.");
                }
            }

            return readValues;
        }

        public bool[] ReadDiscreteReadValuesFromBody()
        {
            string messageString = Encoding.UTF8.GetString(Message);
            string body = messageString.Split(new[] { separator }, StringSplitOptions.None)[1];

            string[] values = body.Split(',');

            bool[] readValues = new bool[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (!bool.TryParse(values[i], out readValues[i]))
                {
                    throw new Exception("Incorrect message formatting: Invalid ReadValues.");
                }
            }

            return readValues;
        }
    }
}

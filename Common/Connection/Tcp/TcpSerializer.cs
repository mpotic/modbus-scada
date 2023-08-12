using Common.Enums;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Connection
{
	public class TcpSerializer : ITcpSerializer
	{
		public const string separator = "//";

		public const string innerSeparator = ";";

		public byte[] Message { get; set; } = new byte[0];

		public bool IsByteArrayTcpSerializedData(byte[] data)
		{
			string input = Encoding.UTF8.GetString(data);
			if(Regex.IsMatch(input, @"^\d*;[^;]*;[^/]*//[^;]*;\d*;[^;]*$"))
			{
				return true;
			}

			return false;
		}

		public void InitMessage()
		{
			Message = Encoding.UTF8.GetBytes(separator);
		}

		public void InitMessage(byte[] message)
		{
			int size = ReadSize(message);
			Message = message.Take(size).ToArray();
		}

		public void AddSizeToHeader()
		{
			byte[] size = Encoding.UTF8.GetBytes((Message.Length + Message.Length.ToString().Length + 1).ToString());
			size = size.Concat(Encoding.UTF8.GetBytes(";")).ToArray();
			Message = size.Concat(Message).ToArray();
		}

		public void AddHeader(SenderCode senderCode, FunctionCode functionCode)
		{
			string header = senderCode.ToString() + ";" + functionCode.ToString();
			byte[] headerBytes = Encoding.UTF8.GetBytes(header);
			Message = headerBytes.Concat(Message).ToArray();
		}

		public void ReplaceSizeInHeader()
		{
			byte[] size = Encoding.UTF8.GetBytes((Message.Length + Message.Length.ToString().Length + 1).ToString());
			size = size.Concat(Encoding.UTF8.GetBytes(";")).ToArray();
			Message = size.Concat(Message).ToArray();
		}

		public void ReplaceHeader(SenderCode senderCode, FunctionCode functionCode)
		{
			string header = senderCode.ToString() + ";" + functionCode + separator;
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

		public void AddBody(byte slaveAddress, ushort startAddress, byte[] writeValues)
		{
			string body = slaveAddress + ";" + startAddress + ";" + string.Join(",", writeValues);
			byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
			Message = Message.Concat(bodyBytes).ToArray();
		}

		public void AddBody(ushort[] values)
		{
			string body = string.Join(",", values);
			byte[] bodyBytes = Encoding.UTF8.GetBytes(";;" + body);
			Message = Message.Concat(bodyBytes).ToArray();
		}

		public void AddBody(bool[] values)
		{
			string body = string.Join(",", values);
			byte[] bodyBytes = Encoding.UTF8.GetBytes(";;" + body);
			Message = Message.Concat(bodyBytes).ToArray();
		}

		public void AddBody(byte[] values)
		{
			string body = string.Join(",", values);
			byte[] bodyBytes = Encoding.UTF8.GetBytes(";;" + body);
			Message = Message.Concat(bodyBytes).ToArray();
		}

		private int ReadSize(byte[] message)
		{
			string messageString = Encoding.UTF8.GetString(message);
			string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
			string[] headerParts = messageParts[0].Split(new[] { innerSeparator }, StringSplitOptions.None);

			if (headerParts.Length < 3 || !int.TryParse(headerParts[0], out int size))
			{
				return -1;
			}

			return size;
		}

		public int ReadSizeFromHeader()
		{
			string messageString = Encoding.UTF8.GetString(Message);
			string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
			string[] headerParts = messageParts[0].Split(new[] { innerSeparator }, StringSplitOptions.None);

			if (headerParts.Length < 3 || !int.TryParse(headerParts[0], out int size))
			{
				return -1;
			}

			return size;
		}

		public SenderCode ReadSenderCodeFromHeader()
		{
			string messageString = Encoding.UTF8.GetString(Message);
			string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
			string[] headerParts = messageParts[0].Split(new[] { innerSeparator }, StringSplitOptions.None);

			SenderCode senderCode = SenderCode.Unknown;
			if (headerParts.Length == 2)
			{
				Enum.TryParse(headerParts[0], out senderCode);
			}
			else if (headerParts.Length == 3)
			{
				Enum.TryParse(headerParts[1], out senderCode);
			}

			return senderCode;
		}

		public FunctionCode ReadFunctionCodeFromHeader()
		{
			string messageString = Encoding.UTF8.GetString(Message);
			string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
			string[] headerParts = messageParts[0].Split(new[] { innerSeparator }, StringSplitOptions.None);

			FunctionCode functionCode = FunctionCode.Unknown;
			if (headerParts.Length == 2)
			{
				Enum.TryParse(headerParts[1], out functionCode);
			}
			else if (headerParts.Length == 3)
			{
				Enum.TryParse(headerParts[2], out functionCode);
			}

			return functionCode;
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

		public ushort[] ReadAnalogReadValuesFromBody()
		{
			string messageString = Encoding.UTF8.GetString(Message);
			string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
			string[] bodyParts = messageParts[1].Split(new[] { innerSeparator }, StringSplitOptions.None);

			if (bodyParts.Length < 3)
			{
				throw new Exception("Incorrect message formatting: Analog values not found.");
			}

			string[] values = bodyParts[2].Split(',');
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

		public byte[] ReadDiscreteValuesFromBody()
		{
			string messageString = Encoding.UTF8.GetString(Message);
			string[] messageParts = messageString.Split(new[] { separator }, StringSplitOptions.None);
			string[] bodyParts = messageParts[1].Split(new[] { innerSeparator }, StringSplitOptions.None);

			if (bodyParts.Length < 3)
			{
				throw new Exception("Incorrect message formatting.");
			}

			string[] values = bodyParts[2].Split(',');
			byte[] writeValues = new byte[values.Length];
			for(int i = 0; i < values.Length; i++)
			{
				if (!byte.TryParse(values[i], out writeValues[i]))
				{
					throw new Exception("Incorrect message formatting.");
				}
			}

			return writeValues;
		}

		public override string ToString()
		{
			return Encoding.UTF8.GetString(Message);
		}
	}
}

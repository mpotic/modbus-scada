using Common.Enums;

namespace Common.Connection
{
	/// <summary>
	/// Initializes a byte array message which at most takes 1024 bytes of storage.
	/// Used for interpreting the packet that represents a request for the modbus slave.
	/// The packet is structured as one of the following: 
	///     write request: "Length;SenderCode;FunctionCode//SlaveAddress;StartAddress;NumberOfPoints",
	///     read request: "Length;SenderCode;FunctionCode//SlaveAddress;StartAddress;WriteValues".
	/// </summary>
	public interface ITcpSerializer
    {
        byte[] Message { get; }

        /// <summary>
        /// Determines using regex whether the data parameter is a sequence that had been previously serialized in the format
        /// that this class serializes in.
        /// </summary>
        bool IsByteArrayTcpSerializedData(byte[] data);

        /// <summary>
        /// Initializes the message with the separator.
        /// </summary>
        void InitMessage();

        /// <summary>
        /// Initializes the message with the existing byte array.
        /// </summary>
        void InitMessage(byte[] message);

        /// <summary>
        /// Adds current size of the message to header.
        /// </summary>
        void AddSizeToHeader();

		/// <summary>
		/// Adds the header to the message.
		/// </summary>
		void AddHeader(SenderCode senderCode, FunctionCode requestCode);

		/// <summary>
		/// Replaces the existing header with given sender code and request code. Previous header is completely removed and replaced with
        /// provided SenderCode and FunctionCode.
		/// </summary>
		void ReplaceHeader(SenderCode senderCode, FunctionCode requestCode);

        /// <summary>
        /// Adds body with slave address, start address, and number of points.
        /// </summary>
        void AddBody(byte slaveAddress, ushort startAddress, ushort numberOfPoints);

        /// <summary>
        /// Adds body with slave address, start address, and ushort array writeValues.
        /// </summary>
        void AddBody(byte slaveAddress, ushort startAddress, ushort[] writeValues);

        /// <summary>
        /// Adds body with slave address, start address, and bool array writeValues.
        /// </summary>
        void AddBody(byte slaveAddress, ushort startAddress, bool[] writeValues);

        /// <summary>
        /// Adds body with slave address, start address, and byte array writeValues.
        /// </summary>
        void AddBody(byte slaveAddress, ushort startAddress, byte[] writeValues);

		/// <summary>
		/// Ads body that contains only the values.
		/// </summary>
		void AddBody(ushort[] values);

        /// <summary>
        /// Ads body that contains only the values.
        /// </summary>
        void AddBody(bool[] values);

		/// <summary>
		/// Ads body that contains only the values.
		/// </summary>
		void AddBody(byte[] values);

		/// <summary>
		/// Reads the size of the message from message header.
		/// </summary>
		/// <returns></returns>
		int ReadSizeFromHeader();

		/// <summary>
		/// Reads SenderCode from the message header.
		/// </summary>
		SenderCode ReadSenderCodeFromHeader();

        /// <summary>
        /// Reads ModbusRequestCode from the message header.
        /// </summary>
        FunctionCode ReadFunctionCodeFromHeader();

        /// <summary>
        /// Reads SlaveAddress from the message body.
        /// </summary>
        byte ReadSlaveAddressFromBody();

        /// <summary>
        /// Reads StartAddress from the message body.
        /// </summary>
        ushort ReadStartAddressFromBody();

        /// <summary>
        /// Reads NumberOfPoints from the message body.
        /// </summary>
        ushort ReadNumberOfPointsFromBody();

        /// <summary>
        /// Reads ushort array WriteValues from the message body.
        /// </summary>
        ushort[] ReadHoldingWriteValuesFromBody();

        /// <summary>
        /// Reads byte array WriteValues from the message body.
        /// </summary>
        byte[] ReadDiscreteValuesFromBody();

        /// <summary>
        /// Reads the analog values previously acquired from slave. 
        /// </summary>
        ushort[] ReadAnalogReadValuesFromBody();

        /// <summary>
        /// Returns the string representation of the Message as UTF8 encoding. 
        /// </summary>
        string ToString();
	}
}

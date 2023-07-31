namespace Common.DTO
{
	/// <summary>
	/// Universal group of parameters to send aong when requesting a modbus service action.
	/// </summary>
	public interface IModbusActionParams
	{
		byte SlaveAddress { get; set; }

		ushort StartAddress { get; set; }

		ushort NumberOfPoints { get; set; }

		byte[] CoilWriteValues { get; set; }

		ushort[] HoldingWriteValues { get; set; }
	}
}

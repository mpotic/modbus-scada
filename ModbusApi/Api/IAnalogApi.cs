﻿using Common.ActionDto;

namespace ModbusApi.Api
{
	public interface IAnalogApi
	{
		void ReadInput(IModbusActionParams actionParams);

		void ReadHolding(IModbusActionParams actionParams);

		void WriteHolding(IModbusActionParams actionParams);
	}
}
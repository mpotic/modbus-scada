﻿using NModbus;
using System.Net.Sockets;

namespace MasterServices.Connection
{
	internal class ModbusConnection : IModbusConnection
	{
		public IModbusMaster ModbusMaster { get; set; }

		public TcpClient ClientConnection { get; set; }
	}
}
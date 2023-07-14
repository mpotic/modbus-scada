﻿using ModbusServices.ServiceProviders;

namespace ModbusServices.Services
{
	public class ServiceHandler : IServiceHandler
	{
		private readonly IConnectionService connectionService;

		private readonly ITcpService tcpService;

		private readonly IModbusProtocolService modbusProtocolService;

		public ServiceHandler()
		{
			connectionService = new ConnectionService(this);
			tcpService = new TcpService(connectionService.StandardConnection);
			modbusProtocolService = new ModbusProtocolService(connectionService.ModbusConnection);

            ConnectionProvider = new ConnectionServiceProvider(connectionService);
            DiscreteProvider = new DiscreteServiceProvider();
			AnalogProvider = new AnalogServiceProvider();
		}

		internal void SetupStandardServices()
		{
			((DiscreteServiceProvider)DiscreteProvider).SetService(tcpService);
			((AnalogServiceProvider)AnalogProvider).SetService(tcpService);
		}

		internal void SetupModbusServices()
		{
			((DiscreteServiceProvider)DiscreteProvider).SetService(modbusProtocolService);
			((AnalogServiceProvider)AnalogProvider).SetService(modbusProtocolService);
		}

		public IConnectionServiceProvider ConnectionProvider { get; private set; }

		public IDiscreteServiceProvider DiscreteProvider { get; private set; }

		public IAnalogServiceProvider AnalogProvider { get; private set; }
	}
}

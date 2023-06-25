using Common.ActionDto;
using ModbusApi;
using ModbusApi.Api;
using ModbusApi.ViewModel;
using ModbusConsole.Callback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusConsole
{
	class Program
	{
		static void Main()
		{
			ModbusApiHandler api = new ModbusApiHandler(new ReadResultsViewModel(), new MessageBoxCallback());
			Console.WriteLine("Modbus API initialized... Waiting to establish the connection...");

			api.ConnectionApi.Connect();

			api.DiscreteApi.ReadInput(new ModbusActionParams(352));

			Console.ReadKey();	
		}
	}
}

using ModbusApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusConsole.Callback
{
	public class MessageBoxCallback : IMessageBoxCallback
	{
		public void DisplayError(string message)
		{
			Console.WriteLine("\n# # # # # # # # # # # # # # # # # # #");
			Console.WriteLine(message);
			Console.WriteLine("# # # # # # # # # # # # # # # # # # #");
		}

		public void DisplaySuccess(string message)
		{
			Console.WriteLine("\n# # # # # # # # # # # # # # # # # # #");
			Console.WriteLine(message);
			Console.WriteLine("# # # # # # # # # # # # # # # # # # #");
		}
	}
}

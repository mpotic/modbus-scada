using Proxy.MessageHandling;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Proxy
{
	internal class Menu : IMenu
	{
		IMessageWorker messageWorker = new MessageWorker();

		Dictionary<int, Task> pendingTasks = new Dictionary<int, Task>();

		public void Begin()
		{
			MenuOptionExecute();
			_ = Task.Run(() => messageWorker.ListenAcceptAndStartReceiving());
		}

		public void BeginAndAutoConnect()
		{
			MenuOptionExecute();
			_ = Task.Run(() => messageWorker.ListenAcceptAndStartReceiving());
			_ = Task.Run(() => messageWorker.AutoConnectAndStartReceiving());
		}

		public void ReadUserInput()
		{
			while (true)
			{
				string read = Console.ReadLine();

				try
				{
					Options(read);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}

		private void Options(string readLine)
		{
			readLine = readLine.Trim().ToLower();

			if (readLine.StartsWith("connect") && readLine.Split(' ').Length == 2)
			{
				ConnectOptionExecute(readLine);
			}
			else if (readLine.StartsWith("disconnect"))
			{
				DisconnectOptionExecute();
			}
			else if (readLine.StartsWith("exit"))
			{
				ExitOptionExecute();
			}
			else if (readLine.StartsWith("listen"))
			{
				ListenOptionExecute();
			}
			else
			{
				MenuOptionExecute();
			}
		}

		private void ConnectOptionExecute(string readLine)
		{
			string portStr = readLine.Split(' ')[1];
			int port = int.Parse(portStr);
			Task.Run(() => messageWorker.ConnectAndStartReceiving(port));
		}

		private void DisconnectOptionExecute()
		{
			messageWorker.Disconnect();
		}

		private void ExitOptionExecute()
		{
			messageWorker.Disconnect();

			Thread.Sleep(10);
			Console.WriteLine("Press any key to exit...");
			Console.ReadLine();

			System.Environment.Exit(0);
		}

		private void ListenOptionExecute()
		{
			Task.Run(() => messageWorker.ListenAcceptAndStartReceiving());
		}

		private void MenuOptionExecute()
		{
			Console.WriteLine("\tMENU\n" +
				"Connect: \"connect portNumber\"\n" +
				"Disconnect: \"disconnect\"\n" +
				"Listen: \"listen\"\n" +
				"Exit: \"exit\"\n");
		}
	}
}

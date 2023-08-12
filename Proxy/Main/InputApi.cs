using Proxy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proxy
{
	internal class InputApi : IInputApi
	{
		private readonly IProxyWorker proxyWorker = new ProxyWorker();

		private readonly IDictionary<string, Action<string>> actions;

		private readonly IDictionary<string, EncryptionTypeCode> encryptionTypes;

		public InputApi()
		{
			actions = new Dictionary<string, Action<string>>
			{
				{ "modbus", Connect },
				{ "tcp", Connect },
				{ "disconnect", Disconnect },
				{ "listen", Listen },
				{ "receive", Receive },
				{ "list", ListAllConnections },
				{ "menu", PrintMenu },
				{ "enc", Encrypt }
			};

			encryptionTypes = new Dictionary<string, EncryptionTypeCode>()
			{
				{ "aes", EncryptionTypeCode.AES },
				{ "none", EncryptionTypeCode.None }
			};
		}

		public void ReadUserInput()
		{
			PrintMenu();
			while (true)
			{
				string read = Console.ReadLine();
				Task.Run(() => { ProcessInput(read); });
			}
		}

		public void ProcessInput(string input)
		{
			input = input.Trim().ToLower();
			string command = GetInputCommand(input);
			Action<string> action;

			try
			{
				actions.TryGetValue(command, out action);
				if (action == null)
				{
					throw new Exception("Invalid command! Enter \"menu\" for the list of commands.");
				}
				actions[command].Invoke(input);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private string[] GetInputParams(string input)
		{
			return input.Split(new char[] { ' ' }, 2)[1].Split(' ').Select(x => x.Trim()).ToArray();
		}

		private string GetInputCommand(string input)
		{
			return input.Split(new char[] { ' ' }, 2)[0].Trim();
		}

		public void Connect(string input)
		{
			string[] inputParams = GetInputParams(input);
			string protocol = GetInputCommand(input);
			int localPort = int.Parse(inputParams[0]);
			int remotePort = int.Parse(inputParams[1]);
			proxyWorker.Connect(protocol, localPort, remotePort);
		}

		public void Disconnect(string input)
		{
			string[] inputParams = GetInputParams(input);
			int localPort = int.Parse(inputParams[0]);
			proxyWorker.Disconnect(localPort);
		}

		public void Listen(string input)
		{
			string[] inputParams = GetInputParams(input);
			int listenPort = int.Parse(inputParams[0]);
			proxyWorker.Listen(listenPort);
		}

		public void Receive(string input)
		{
			string[] inputParams = GetInputParams(input);
			int localPort = int.Parse(inputParams[0]);
			int remotePort = int.Parse(inputParams[1]);
			proxyWorker.Receive(localPort, remotePort);
		}

		public void ListAllConnections(string input = null)
		{
			proxyWorker.ListAllConections();
		}

		public void Encrypt(string input)
		{
			string[] inputParams = GetInputParams(input);
			encryptionTypes.TryGetValue(inputParams[0], out EncryptionTypeCode encryptionType);
			proxyWorker.ConfigureEncryption(encryptionType);
		}

		private void PrintMenu(string input = null)
		{
			Console.WriteLine(
				"- - - - - - - - - - - - M E N U - - - - - - - - - - - -\n" +
				"Connect: \"{modbus/tcp} {localPort} {remotePort}\"\n" +
				"Disconnect: \"disconnect {localPort}\"\n" +
				"Listen: \"listen {localPort}\"\n" +
				"Receive\\Send: \"receive {receivePort} {sendPort}\"\n" +
				"List ports in use: \"list\"\n" +
				"Encrypt: \"enc {algorithm}\"\n" +
				"Sign: \"sign {true/false}\"\n" +
			 	"Exit: \"exit\"\n" +
				"- - - - - - - - - - - - - - - - - - - - - - - - - - - -");
		}
	}
}

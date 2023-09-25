using Proxy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proxy
{
	internal class InputApi : IInputApi
	{
		private readonly IProxyWorker proxyWorker;

		private readonly ISecurityHandler security = new SecurityHandler();

		private readonly IDictionary<string, Action<string>> actions;

		private readonly IDictionary<string, EncryptionTypeCode> encryptionTypes;

		public InputApi()
		{
			actions = new Dictionary<string, Action<string>>
			{
				{ "modbus", Connect },
				{ "tcp", Connect },
				{ "listen", Listen },
				{ "disc", Disconnect },
				{ "master", ReceiveMaster },
				{ "proxy", ReceiveProxy },
				{ "list", ListAllConnections },
				{ "enc", Encrypt },
				{ "sign", Sign },
				{ "cert", Certificate },
				{ "cls", Clear }
			};

			encryptionTypes = new Dictionary<string, EncryptionTypeCode>()
			{
				{ "aes", EncryptionTypeCode.AES },
				{ "none", EncryptionTypeCode.None }
			};

			proxyWorker = new ProxyWorker(security);
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
                    Console.WriteLine("Invalid command!");
					PrintMenu();

					return;
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

		public void ReceiveMaster(string input)
		{
			string[] inputParams = GetInputParams(input);
			int localPort = int.Parse(inputParams[0]);
			int remotePort = int.Parse(inputParams[1]);
			proxyWorker.ReceiveMaster(localPort, remotePort);
		}

		public void ReceiveProxy(string input)
		{
			string[] inputParams = GetInputParams(input);
			int localPort = int.Parse(inputParams[0]);
			int remotePort = int.Parse(inputParams[1]);
			proxyWorker.ReceiveProxy(localPort, remotePort);
		}

		public void ListAllConnections(string input = null)
		{
			proxyWorker.ListAllConections();
		}

		public void Encrypt(string input)
		{
			string[] inputParams = GetInputParams(input);
			if (!encryptionTypes.TryGetValue(inputParams[0], out EncryptionTypeCode encryptionType))
			{
				throw new Exception("Invalid encryption type!");
			}
			security.ConfigureEncryption(encryptionType);
		}

		public void Sign(string input)
		{
			string[] inputParams = GetInputParams(input);
			if(inputParams.Length < 0)
			{
				throw new Exception("No sign option was provided {true/false}!");
			}

			bool isSign = bool.Parse(inputParams[0]);
			security.ConfigureSigning(isSign);
		}

		public void Certificate(string input)
		{
			string[] inputParams = GetInputParams(input);
			if (inputParams[0] == "make")
			{
				security.GenerateCert();
			}
			else if (inputParams[0] == "load")
			{
				security.LoadCert();
			}
			else
			{
				throw new Exception("Invalid certificate option!");
			}
		}

		public void Clear(string input)
		{
			Console.Clear();
		}

		private void PrintMenu(string input = null)
		{
			Console.WriteLine(
				"- - - - - - - - - - - M E N U - - - - - - - - - - -\n" +
				"Connect: \"{modbus/tcp} {localPort} {remotePort}\"\n" +
				"Listen: \"listen {localPort}\"\n" +
				"Disconnect: \"disc {localPort}\"\n" +
				"Receive\\Send: \"{master/proxy} {receivePort} {sendPort}\"\n" +
				"List ports in use: \"list\"\n" +
				"Encrypt messages: \"enc {algorithm/none}\"\n" +
				"Sign messages: \"sign {true/false}\"\n" +
				"Create certificates: \"cert make\"\n" +
				"Load certificates: \"cert load\"\n" +
				"Clear console: \"cls\"\n" +
				"- - - - - - - - - - - - - - - - - - - - - - - - - -");
		}
	}
}

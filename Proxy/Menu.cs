using Proxy.MessageHandling;
using System;
using System.Threading.Tasks;

namespace Proxy
{
    internal class Menu : IMenu
    {
        IMessageWorker messageWorker = new MessageWorker();

        public async void Begin()
        {
           await Task.Run(() => messageWorker.AcceptAndStartReceiving());
        }

        public async void BeginAndAutoConnect()
        {
            await Task.Run(() => messageWorker.AcceptAndStartReceiving());
            _ = Task.Run(() => messageWorker.AutoConnectAndStartReceiving());
        }

        public void ReadInput()
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

            if (readLine.StartsWith("con") && readLine.Split(' ').Length == 2)
            {
                string portStr = readLine.Split(' ')[1];
                int port = int.Parse(portStr);
                Task.Run(() => messageWorker.ConnectAndStartReceiving(port));
            }
            else if (readLine.StartsWith("exit"))
            {
                messageWorker.Disconnect();

                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();

                System.Environment.Exit(0);
            }
        }
    }
}

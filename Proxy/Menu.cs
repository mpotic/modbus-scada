using Proxy.MessageHandling;
using System;
using System.Threading.Tasks;

namespace Proxy
{
    internal class Menu
    {
        IMessageWorker messageWorker = new MessageWorker();

        public void Begin()
        {
            Task.Run(() => messageWorker.AcceptAndStartReceiving());
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
                System.Environment.Exit(0);
            }
        }
    }
}

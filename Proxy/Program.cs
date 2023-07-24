namespace Proxy
{
	class Program
	{
		static void Main(string[] args)
		{
			IMenu menu = new Menu();

			menu.BeginAndAutoConnect();
			
			menu.ReadUserInput();
		}
	}
}

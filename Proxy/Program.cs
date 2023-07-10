using System;

namespace Proxy
{
    class Program
	{
		static void Main(string[] args)
		{
			Menu menu = new Menu();

			menu.Begin();
			
			menu.ReadInput();
		}
	}
}

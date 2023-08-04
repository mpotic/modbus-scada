namespace Proxy
{
	class Program
	{
		static void Main(string[] args)
		{
			IInputApi inputApi = new InputApi();

			inputApi.ReadUserInput();
		}
	}
}

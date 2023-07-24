namespace Proxy
{
    internal interface IMenu
	{
		/// <summary>
		/// Calls a method on another thread to listen and accept a single connection attempt and start receiving the messages.
		/// </summary>
		void Begin();

		/// <summary>
		/// Calls a method on another thread to listen and accept a single connection attempt and start receiving the messages.
		/// Tries to auto connect to another proxy on another thread if both proxies are running.
		/// </summary>
		void BeginAndAutoConnect();

		void ReadUserInput();
	}
}

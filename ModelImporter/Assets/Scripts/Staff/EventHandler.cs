namespace Staff
{
	public delegate void EventHandler<T>(T sender);
	public delegate void EventHandler<T, V>(T sender, V value);
	
	public static class EventHandlerExtended
	{
		public static void Raise<T>(this EventHandler<T> @event, T sender)
		{
			var handler = @event;
			if (handler != null) handler.Invoke(sender);
		}

		public static void Raise<T, V>(this EventHandler<T, V> @event, T sender, V value)
		{
			var handler = @event;
			if (handler != null) handler.Invoke(sender, value);
		}
		
	}
}
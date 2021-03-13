using System;
using System.Collections.Generic;

namespace Diamond.Core.Example.Wpf
{
	public class SimpleBus : DisposableObject, ISimpleBus
	{
		protected IList<ISubscriber> Subscribers { get; } = new List<ISubscriber>();

		public void Subscribe(ISubscriber subscriber)
		{
			this.Subscribers.Add(subscriber);
		}

		public void Publish(IMessage message)
		{
			foreach (ISubscriber subscriber in this.Subscribers)
			{
				try
				{
					subscriber.OnMessageAsync(message);
				}
				catch
				{
				}
			}
		}

		protected override void OnDisposeManagedObjects()
		{
			this.Subscribers.Clear();
		}
	}
}

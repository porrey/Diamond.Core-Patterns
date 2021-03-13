namespace Diamond.Core.Example.Wpf
{
	public interface ISimpleBus
	{
		void Publish(IMessage message);
		void Subscribe(ISubscriber subscriber);
	}
}
using System.Threading.Tasks;

namespace Diamond.Core.Example.Wpf
{
	public interface ISubscriber
	{
		Task OnMessageAsync(IMessage message);
	}
}

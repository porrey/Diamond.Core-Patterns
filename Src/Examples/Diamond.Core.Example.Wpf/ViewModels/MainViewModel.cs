using System.ComponentModel;
using System.Threading.Tasks;

namespace Diamond.Core.Example.Wpf
{
	public class MainViewModel : IMainViewModel, ISubscriber, INotifyPropertyChanged
	{
		public MainViewModel(ISimpleBus simpleBus)
		{
			this.SimpleBus = simpleBus;
			this.SimpleBus.Subscribe(this);
		}

		protected ISimpleBus SimpleBus { get; set; }
		public int Counter { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public Task OnMessageAsync(IMessage message)
		{
			this.Counter = message.Counter;

			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Counter)));

			return Task.FromResult(0);
		}
	}
}

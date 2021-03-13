using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Example.Wpf
{
	public class BackgroundProcess : IHostedService
	{
		public BackgroundProcess(ISimpleBus simpleBus)
		{
			this.SimpleBus = simpleBus;
		}

		protected ISimpleBus SimpleBus { get; set; }

		public Task StartAsync(CancellationToken cancellationToken)
		{
			int counter = 0;

			return Task.Factory.StartNew(async () =>
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					this.SimpleBus.Publish(new Message() { Counter = counter });
					await Task.Delay(1000);
					counter++;

					if (counter > 9999)
					{
						counter = 0;
					}
				}
			}, cancellationToken);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}

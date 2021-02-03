using System.Threading.Tasks;
using Diamond.Patterns.Decorator;
using Microsoft.Extensions.Logging;

namespace Diamond.Patterns.Example
{
	public class BookTransactionDecorator : IDecorator<IBook, bool>
	{
		private readonly ILogger<BookTransactionDecorator> _logger = null;

		public BookTransactionDecorator(ILogger<BookTransactionDecorator> logger)
		{
			_logger = logger;
		}

		public string Name { get; set; } = WellKnown.Decorator.BookTransaction;
		
		public Task<bool> TakeActionAsync(IBook item)
		{
			bool returnValue = false;

			if (item.CheckedOut)
			{
				_logger.LogWarning($"The book '{item.Title}' is already checked out.");
				returnValue = false;
			}
			else
			{
				item.CheckedOut = true;
				returnValue = true;
				_logger.LogWarning($"The book '{item.Title}' was successfully checked out.");
			}

			return Task.FromResult(returnValue);
		}
	}
}

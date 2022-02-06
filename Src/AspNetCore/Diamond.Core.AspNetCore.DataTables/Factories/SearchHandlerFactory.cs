using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.AspNetCore.DataTables
{
	public class SearchHandlerFactory<TEntity> : ISearchHandlerFactory<TEntity>
	{
		public SearchHandlerFactory(ILogger<SearchHandlerFactory<TEntity>> logger, IServiceProvider serviceProvider)
		{
			this.Logger = logger;
			this.ServiceProvider = serviceProvider;
		}

		protected ILogger<SearchHandlerFactory<TEntity>> Logger { get; set; }
		protected IServiceProvider ServiceProvider { get; set; }

		public Task<ISearchHandler<TEntity>> GetAsync(string propertyName)
		{
			ISearchHandler<TEntity> returnValue = null;

			//
			// Get the handlers.
			//
			IEnumerable<ISearchHandler<TEntity>> handlers = this.ServiceProvider.GetService<IEnumerable<ISearchHandler<TEntity>>>();

			//
			// Get the specific handler.
			//
			returnValue = handlers.Where(t => t.PropertyName.ToLower() == propertyName.ToLower()).FirstOrDefault();

			return Task.FromResult(returnValue);
		}
	}
}

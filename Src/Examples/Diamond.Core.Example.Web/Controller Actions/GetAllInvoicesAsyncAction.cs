using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.AspNet.DoAction;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class GetAllInvoicesAsyncAction : IDoAction<object, IControllerActionResult<IEnumerable<InvoiceItem>>>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		public GetAllInvoicesAsyncAction(ILogger<GetAllInvoicesAsyncAction> logger, IRepositoryFactory repositoryFactory)
		{
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<GetAllInvoicesAsyncAction> Logger { get; set; }

		/// <summary>
		/// Get the action key for this action. The action should match the name of the method
		/// it handles, and the method control should use nameof() when specifying the key (as
		/// a best pratice, not required).
		/// </summary>
		protected IRepositoryFactory RepositoryFactory { get; set; }

		/// <summary>
		/// As a best pratice, the name of this class should match the controller
		/// method name with the word "Action" appended to the end. The DoActionController
		/// uses [CallerMemberName] as the action key by default.
		/// </summary>
		public string ActionKey => typeof(GetAllInvoicesAsyncAction).Name.Replace("Action", "");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<IControllerActionResult<IEnumerable<InvoiceItem>>> ExecuteActionAsync(object item)
		{
			ControllerActionResult<IEnumerable<InvoiceItem>> returnValue = new ControllerActionResult<IEnumerable<InvoiceItem>>();

			// ***
			// *** Get a read-only repository for IInvoice.
			// ***
			this.Logger.LogTrace("Retrieving read-only repository for IInvoice.");
			IReadOnlyRepository<IInvoice> repository = await this.RepositoryFactory.GetReadOnlyAsync<IInvoice>();

			// ***
			// *** Query all of the items and create a InvoiceReponse for each.
			// ***
			this.Logger.LogTrace("Retrieving all IInvoice items from data storage.");
			IEnumerable<InvoiceItem> items = (from tbl in await repository.GetAllAsync()
											  select tbl).ToInvoiceItemList();

			if (items.Any())
			{
				this.Logger.LogTrace($"There were {items.Count()} IInvoice items retrieved.");
				returnValue.ResultType = ResultType.Ok;
				returnValue.ErrorMessage = null;
				returnValue.Result = items;
			}
			else
			{
				string message = "There are no invoice items in the ERP system.";
				this.Logger.LogTrace(message);
				returnValue.ResultType = ResultType.NotFound;
				returnValue.ErrorMessage = message;
				returnValue.Result = null;
			}

			return returnValue;
		}
	}
}

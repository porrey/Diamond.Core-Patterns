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
	public class GetInvoiceAsyncAction : IDoAction<string, IControllerActionResult<InvoiceItem>>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		public GetInvoiceAsyncAction(ILogger<CreateInvoiceAsyncAction> logger, IRepositoryFactory repositoryFactory)
		{
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<CreateInvoiceAsyncAction> Logger { get; set; }

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
		public string ActionKey => typeof(GetInvoiceAsyncAction).Name.Replace("Action", "");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invoiceNumber"></param>
		/// <returns></returns>
		public async Task<IControllerActionResult<InvoiceItem>> ExecuteActionAsync(string invoiceNumber)
		{
			ControllerActionResult<InvoiceItem> returnValue = new ControllerActionResult<InvoiceItem>();

			// ***
			// *** Get a read-only repository for IInvoice.
			// ***
			this.Logger.LogTrace("Retrieving a read-only repository for IInvoice.");
			IReadOnlyRepository<IInvoice> repository = await this.RepositoryFactory.GetReadOnlyAsync<IInvoice>();

			// ***
			// *** Attempt to create the item.
			// ***
			IInvoice item = (await repository.GetAsync(t => t.Number == invoiceNumber)).SingleOrDefault();

			if (item != null)
			{
				returnValue.ResultType = ResultType.Ok;
				returnValue.ErrorMessage = null;
				returnValue.Result = item.FromEntity();
			}
			else
			{
				returnValue.ResultType = ResultType.NotFound;
				returnValue.ErrorMessage = $"An invoice with invoice number '{invoiceNumber}' could not be found.";
				returnValue.Result = null;
			}

			return returnValue;
		}
	}
}

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
	public class UpdateInvoiceAsyncAction : IDoAction<InvoiceItem, IControllerActionResult<InvoiceItem>>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		public UpdateInvoiceAsyncAction(ILogger<UpdateInvoiceAsyncAction> logger, IRepositoryFactory repositoryFactory)
		{
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<UpdateInvoiceAsyncAction> Logger { get; set; }

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
		public string ActionKey => typeof(UpdateInvoiceAsyncAction).Name.Replace("Action", "");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<IControllerActionResult<InvoiceItem>> ExecuteActionAsync(InvoiceItem item)
		{
			ControllerActionResult<InvoiceItem> returnValue = new ControllerActionResult<InvoiceItem>();

			// ***
			// *** Get a writable repository for IInvoice.
			// ***
			this.Logger.LogTrace("Retrieving a writable repository for IInvoice.");
			IWritableRepository<IInvoice> repository = await this.RepositoryFactory.GetWritableAsync<IInvoice>();

			// ***
			// *** Get the invoice.
			// ***
			IInvoice exisingItem = (await repository.GetAsync(t => t.Id == item.Id)).SingleOrDefault();

			if (exisingItem != null)
			{
				// ***
				// *** Update the existing item.
				// ***
				exisingItem.Update(item);

				// ***
				// *** Update the data.
				// ***
				bool result = await repository.UpdateAsync(exisingItem);

				if (result)
				{
					returnValue.ResultType = ResultType.Ok;
					returnValue.ErrorMessage = null;
					returnValue.Result = exisingItem.FromEntity();
				}
				else
				{
					returnValue.ResultType = ResultType.BadRequest;
					returnValue.ErrorMessage = $"The invoice with invoice number '{item.Number}' could not be updated.";
					returnValue.Result = null;
				}
			}
			else
			{
				returnValue.ResultType = ResultType.NotFound;
				returnValue.ErrorMessage = $"An invoice with invoice number '{item.Number}' could not be found.";
				returnValue.Result = null;
			}

			return returnValue;
		}
	}
}

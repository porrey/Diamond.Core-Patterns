using System.Threading.Tasks;
using Diamond.Core.AspNet.DoAction;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class CreateInvoiceAsyncAction : IDoAction<InvoiceItem, IControllerActionResult<InvoiceItem>>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		public CreateInvoiceAsyncAction(ILogger<CreateInvoiceAsyncAction> logger, IRepositoryFactory repositoryFactory)
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
		public string ActionKey => typeof(CreateInvoiceAsyncAction).Name.Replace("Action", "");

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
			// *** Create a new entity.
			// ***
			IInvoice model = await repository.ModelFactory.CreateAsync();

			// ***
			// *** Set the properties.
			// ***
			item.CopyTo(model);

			// ***
			// *** Attempt to create the item.
			// ***
			(bool result, IInvoice newItem) = await repository.AddAsync(model);

			if (result)
			{
				returnValue.ResultType = ResultType.Ok;
				returnValue.ErrorMessage = null;
				returnValue.Result = newItem.FromEntity();
			}
			else
			{
				returnValue.ResultType = ResultType.BadRequest;
				returnValue.ErrorMessage = "Could not create invoice item.";
				returnValue.Result = null;
			}

			return returnValue;
		}
	}
}

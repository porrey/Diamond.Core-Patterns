using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Diamond.Core.AspNet.DoAction;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class DeleteInvoiceAsyncAction : IDoAction<string, IControllerActionResult<Invoice>>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public DeleteInvoiceAsyncAction(ILogger<DeleteInvoiceAsyncAction> logger, IRepositoryFactory repositoryFactory, IMapper mapper)
		{
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;
			this.Mapper = mapper;
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<DeleteInvoiceAsyncAction> Logger { get; set; }

		/// <summary>
		/// Holds the reference to <see cref="IRepositoryFactory"/>.
		/// </summary>
		protected IRepositoryFactory RepositoryFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected IMapper Mapper { get; set; }

		/// <summary>
		/// As a best practice, the name of this class should match the controller
		/// method name with the word "Action" appended to the end. The DoActionController
		/// uses [CallerMemberName] as the action key by default.
		/// </summary>
		public string ActionKey => typeof(DeleteInvoiceAsyncAction).Name.Replace("Action", "");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invoiceNumber"></param>
		/// <returns></returns>
		public async Task<IControllerActionResult<Invoice>> ExecuteActionAsync(string invoiceNumber)
		{
			ControllerActionResult<Invoice> returnValue = new ControllerActionResult<Invoice>();

			// ***
			// *** Get a writable repository for IInvoice.
			// ***
			this.Logger.LogTrace("Retrieving a writable repository for IInvoice.");
			IWritableRepository<IInvoice> repository = await this.RepositoryFactory.GetWritableAsync<IInvoice>();

			// ***
			// *** Get the invoice.
			// ***
			IInvoice exisingItem = (await repository.GetAsync(t => t.Number == invoiceNumber)).SingleOrDefault();

			if (exisingItem != null)
			{
				// ***
				// *** Update the data.
				// ***
				bool result = await repository.DeleteAsync(exisingItem);

				if (result)
				{
					returnValue.ResultDetails = DoActionResult.CreateOk();
					returnValue.Result = this.Mapper.Map<Invoice>(exisingItem);
				}
				else
				{
					returnValue.ResultDetails = DoActionResult.CreateBadRequest($"The invoice with invoice number '{invoiceNumber}' could not be deleted.");
				}
			}
			else
			{
				returnValue.ResultDetails = DoActionResult.CreateNotFound($"An invoice with invoice number '{invoiceNumber}' could not be found.");
			}

			return returnValue;
		}
	}
}

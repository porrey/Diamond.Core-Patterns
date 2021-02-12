using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Diamond.Core.AspNet.DoAction;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example {
	/// <summary>
	/// 
	/// </summary>
	public class GetInvoiceAsyncAction : DoActionAsync<string, Invoice> {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public GetInvoiceAsyncAction(ILogger<GetInvoiceAsyncAction> logger, IRepositoryFactory repositoryFactory, IMapper mapper)
			: base(logger) {
			this.RepositoryFactory = repositoryFactory;
			this.Mapper = mapper;
		}

		/// <summary>
		/// Holds the reference to <see cref="IRepositoryFactory"/>.
		/// </summary>
		protected IRepositoryFactory RepositoryFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected IMapper Mapper { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invoiceNumber"></param>
		/// <returns></returns>
		protected override async Task<IControllerActionResult<Invoice>> OnExecuteActionAsync(string invoiceNumber) {
			ControllerActionResult<Invoice> returnValue = new ControllerActionResult<Invoice>();

			//
			// Get a read-only repository for IInvoice.
			//
			this.Logger.LogTrace("Retrieving a read-only repository for IInvoice.");
			IReadOnlyRepository<IInvoice> repository = await this.RepositoryFactory.GetReadOnlyAsync<IInvoice>();

			//
			// Attempt to create the item.
			//
			IInvoice item = (await repository.GetAsync(t => t.Number == invoiceNumber)).SingleOrDefault();

			if (item != null) {
				returnValue.ResultDetails = DoActionResult.Ok();
				returnValue.Result = this.Mapper.Map<Invoice>(item);
			}
			else {
				IDictionary<string, object> extensions = new Dictionary<string, object>
				{
					{ "Number", invoiceNumber }
				};

				returnValue.ResultDetails = DoActionResult.NotFound($"An invoice with invoice number '{invoiceNumber}' could not be found.", null, null, extensions);
			}

			return returnValue;
		}
	}
}

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
	public class GetAllInvoicesAsyncAction : DoActionAsync<object, IEnumerable<Invoice>> {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public GetAllInvoicesAsyncAction(ILogger<GetAllInvoicesAsyncAction> logger, IRepositoryFactory repositoryFactory, IMapper mapper) 
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
		/// <param name="item"></param>
		/// <returns></returns>
		protected override async Task<IControllerActionResult<IEnumerable<Invoice>>> OnExecuteActionAsync(object item) {
			ControllerActionResult<IEnumerable<Invoice>> returnValue = new ControllerActionResult<IEnumerable<Invoice>>();

			//
			// Get a read-only repository for IInvoice.
			//
			this.Logger.LogTrace("Retrieving read-only repository for IInvoice.");
			IReadOnlyRepository<IInvoice> repository = await this.RepositoryFactory.GetReadOnlyAsync<IInvoice>();

			//
			// Query all of the items and create a InvoiceReponse for each.
			//
			this.Logger.LogTrace("Retrieving all IInvoice items from data storage.");
			IEnumerable<Invoice> items = from tbl in await repository.GetAllAsync()
										 select this.Mapper.Map<Invoice>(tbl);

			if (items.Any()) {
				this.Logger.LogTrace($"There were {items.Count()} IInvoice items retrieved.");
				returnValue.ResultDetails = DoActionResult.Ok();
				returnValue.Result = items;
			}
			else {
				returnValue.ResultDetails = DoActionResult.NotFound("There are no invoices in the ERP system.");
			}

			return returnValue;
		}
	}
}

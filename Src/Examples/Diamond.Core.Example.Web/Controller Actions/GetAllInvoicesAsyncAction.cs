using System.Collections.Generic;
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
	public class GetAllInvoicesAsyncAction : IDoAction<object, IControllerActionResult<IEnumerable<Invoice>>>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public GetAllInvoicesAsyncAction(ILogger<GetAllInvoicesAsyncAction> logger, IRepositoryFactory repositoryFactory, IMapper mapper)
		{
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;
			this.Mapper = mapper;
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<GetAllInvoicesAsyncAction> Logger { get; set; }

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
		public string ActionKey => typeof(GetAllInvoicesAsyncAction).Name.Replace("Action", "");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<IControllerActionResult<IEnumerable<Invoice>>> ExecuteActionAsync(object item)
		{
			ControllerActionResult<IEnumerable<Invoice>> returnValue = new ControllerActionResult<IEnumerable<Invoice>>();

			// ***
			// *** Get a read-only repository for IInvoice.
			// ***
			this.Logger.LogTrace("Retrieving read-only repository for IInvoice.");
			IReadOnlyRepository<IInvoice> repository = await this.RepositoryFactory.GetReadOnlyAsync<IInvoice>();

			// ***
			// *** Query all of the items and create a InvoiceReponse for each.
			// ***
			this.Logger.LogTrace("Retrieving all IInvoice items from data storage.");
			IEnumerable<Invoice> items = from tbl in await repository.GetAllAsync()
											 select this.Mapper.Map<Invoice>(tbl);

			if (items.Any())
			{
				this.Logger.LogTrace($"There were {items.Count()} IInvoice items retrieved.");
				returnValue.ResultDetails = DoActionResult.CreateOk();
				returnValue.Result = items;
			}
			else
			{
				returnValue.ResultDetails = DoActionResult.CreateNotFound("There are no invoices in the ERP system.");
			}

			return returnValue;
		}
	}
}

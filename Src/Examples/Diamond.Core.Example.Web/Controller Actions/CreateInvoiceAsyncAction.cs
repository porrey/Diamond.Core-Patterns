using System.Threading.Tasks;
using AutoMapper;
using Diamond.Core.AspNet.DoAction;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example {
	/// <summary>
	/// 
	/// </summary>
	public class CreateInvoiceAsyncAction : IDoAction<Invoice, IControllerActionResult<Invoice>> {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public CreateInvoiceAsyncAction(ILogger<CreateInvoiceAsyncAction> logger, IRepositoryFactory repositoryFactory, IMapper mapper) {
			this.Logger = logger;
			this.RepositoryFactory = repositoryFactory;
			this.Mapper = mapper;
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<CreateInvoiceAsyncAction> Logger { get; set; }

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
		public string ActionKey => typeof(CreateInvoiceAsyncAction).Name.Replace("Action", "");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<IControllerActionResult<Invoice>> ExecuteActionAsync(Invoice item) {
			ControllerActionResult<Invoice> returnValue = new ControllerActionResult<Invoice>();

			//
			// Get a writable repository for IInvoice.
			//
			this.Logger.LogTrace("Retrieving a writable repository for IInvoice.");
			IWritableRepository<IInvoice> repository = await this.RepositoryFactory.GetWritableAsync<IInvoice>();

			//
			// Create a new entity.
			//
			IInvoice model = await repository.ModelFactory.CreateAsync();

			//
			// Set the properties.
			//
			this.Mapper.Map(item, model);

			//
			// Attempt to create the item.
			//
			(bool result, IInvoice newItem) = await repository.AddAsync(model);

			if (result) {
				returnValue.ResultDetails = DoActionResult.Created();
				returnValue.Result = this.Mapper.Map<Invoice>(newItem);
			}
			else {
				returnValue.ResultDetails = DoActionResult.BadRequest("Could not create invoice.");
			}

			return returnValue;
		}
	}
}

using System.Threading.Tasks;
using AutoMapper;
using Diamond.Core.AspNetCore.DoAction;
using Diamond.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class CreateInvoiceAsyncAction : DoActionTemplate<Invoice, Invoice>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repositoryFactory"></param>
		/// <param name="mapper"></param>
		public CreateInvoiceAsyncAction(ILogger<CreateInvoiceAsyncAction> logger, IRepositoryFactory repositoryFactory, IMapper mapper)
			: base(logger)
		{
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
		protected override async Task<IControllerActionResult<Invoice>> OnExecuteActionAsync(Invoice item)
		{
			ControllerActionResult<Invoice> returnValue = new ControllerActionResult<Invoice>();

			//
			// Get a writable repository for IInvoice.
			//
			this.Logger.LogDebug("Retrieving a writable repository for IInvoice.");
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

			if (result)
			{
				returnValue.ResultDetails = DoActionResult.Created();
				returnValue.Result = this.Mapper.Map<Invoice>(newItem);
			}
			else
			{
				returnValue.ResultDetails = DoActionResult.BadRequest("Could not create invoice.");
			}

			return returnValue;
		}
	}
}

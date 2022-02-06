using Diamond.Core.AspNetCore.DoAction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.AspNetCore.DataTables
{
	[ApiController]
	[Route("[controller]")]
	public abstract class DataTableControllerTemplate<TViewModel, TRequest> : DoActionController
	{
		public DataTableControllerTemplate(IDoActionFactory doActionFactory, ILogger<DataTableControllerTemplate<TViewModel, TRequest>> logger)
			: base(doActionFactory, logger)
		{
		}

		[HttpPost("form")]
		[Consumes("application/x-www-form-urlencoded")]
		public Task<ActionResult<DataTableResult<TViewModel>>> HistoryGridAsync([FromForm] IFormCollection request)
		{
			return this.Do<IFormCollection, DataTableResult<TViewModel>>(request);
		}

		[HttpPost("data")]
		[Consumes("application/json")]
		public Task<ActionResult<DataTableResult<TViewModel>>> HistoryGridAsync([FromBody] TRequest request)
		{
			return this.Do<TRequest, DataTableResult<TViewModel>>(request);
		}
	}
}

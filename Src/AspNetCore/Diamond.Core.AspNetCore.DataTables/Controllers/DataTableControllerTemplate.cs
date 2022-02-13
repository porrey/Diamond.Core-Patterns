//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
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
		public virtual Task<ActionResult<DataTableResult<TViewModel>>> DataTableAsync([FromForm] IFormCollection request)
		{
			return this.Do<IFormCollection, DataTableResult<TViewModel>>(request);
		}

		[HttpPost("data")]
		[Consumes("application/json")]
		public virtual Task<ActionResult<DataTableResult<TViewModel>>> DataTableAsync([FromBody] TRequest request)
		{
			return this.Do<TRequest, DataTableResult<TViewModel>>(request);
		}
	}
}

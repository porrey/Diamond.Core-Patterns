// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.AspNet.DoAction
{
	/// <summary>
	/// Defines a generic repository factory that can be used to retrieve
	/// an object that implements IDecorator<TItem, TResult> from the container.
	/// </summary>
	public class DoActionFactory : IDoActionFactory
	{
		public DoActionFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		public ILogger<DoActionFactory> Logger { get; set; } = new NullLogger<DoActionFactory>();

		protected IServiceProvider ServiceProvider { get; set; }

		public Task<IDoAction<TInputs, TResult>> GetAsync<TInputs, TResult>(string actionKey)
		{
			IDoAction<TInputs, TResult> returnValue = null;

			// ***
			// *** Get the decorator type being requested.
			// ***
			Type targetType = typeof(IDoAction<TInputs, TResult>);
			this.Logger.LogTrace($"Finding an IDoAction of type '{targetType.Name}' and action key of '{actionKey}'.");

			// ***
			// *** Get all decorators from the container of
			// *** type IDecorator<TItem>.
			// ***
			IEnumerable<IDoAction> items = this.ServiceProvider.GetRequiredService<IEnumerable<IDoAction>>();
			IDoAction doAction = items.Where(t => t.ActionKey == actionKey).FirstOrDefault();

			// ***
			// *** Within the list, find the target decorator.
			// ***
			if (doAction != null)
			{
				if (targetType.IsInstanceOfType(doAction))
				{
					this.Logger.LogTrace($"IDecorator of type '{targetType.Name}' and action key of '{actionKey}' was found.");
					returnValue = (IDoAction<TInputs, TResult>)doAction;
				}
				else
				{
					this.Logger.LogError($"IDecorator of type '{targetType.Name}' and action key of '{actionKey}' was NOT found. Throwing exception...");
					throw new DoActionNotFoundException<TInputs, TResult>(actionKey);
				}
			}
			else
			{
				this.Logger.LogError($"IDecorator of type '{targetType.Name}' and action key of '{actionKey}' was NOT found. Throwing exception...");
				throw new DoActionNotFoundException<TInputs, TResult>(actionKey);
			}

			return Task.FromResult(returnValue);
		}
	}
}

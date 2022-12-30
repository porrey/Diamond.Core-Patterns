//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.AspNetCore.DoAction
{
	/// <summary>
	/// A factory that can be used to retrieve a specific <see cref="DoActionTemplate{TInputs, TResult}"/>
	/// from a container.
	/// </summary>
	public class DoActionFactory : IDoActionFactory
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		public DoActionFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// Gets/sets the instance of the logger used by the factory. The default is a null logger.
		/// </summary>
		public virtual ILogger<DoActionFactory> Logger { get; set; } = new NullLogger<DoActionFactory>();

		/// <summary>
		/// Gets/sets the <see cref="IServiceProvider"/> used by the factory to retrieve 
		/// the specification instances.
		/// </summary>
		protected virtual IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// Gets an instance of <see cref="IDoAction{TInputs, TResult}"/> from the container.
		/// </summary>
		/// <typeparam name="TInputs">The type of input defined by the action.</typeparam>
		/// <typeparam name="TResult">The result type defined by the action.</typeparam>
		/// <param name="actionKey">A unique key used to identify a specific action.</param>
		/// <returns></returns>
		public virtual Task<IDoAction<TInputs, TResult>> GetAsync<TInputs, TResult>(string actionKey)
		{
			IDoAction<TInputs, TResult> returnValue = null;

			//
			// Get the decorator type being requested.
			//
			Type targetType = typeof(IDoAction<TInputs, TResult>);
			this.Logger.LogDebug("Finding an IDoAction of type '{targetType}' and action key of '{actionKey}'.", targetType.Name, actionKey);

			//
			// Get all decorators from the container of
			// type IDecorator<TItem>.
			//
			IEnumerable<IDoAction> items = this.ServiceProvider.GetRequiredService<IEnumerable<IDoAction>>();
			IDoAction doAction = items.Where(t => t.ActionKey == actionKey).FirstOrDefault();
			IEnumerable<IDoAction> unusedItems = items.Except(new IDoAction[] { doAction });

			//
			// Within the list, find the target decorator.
			//
			if (doAction != null)
			{
				if (targetType.IsInstanceOfType(doAction))
				{
					this.Logger.LogDebug("IDecorator of type '{targetType}' and action key of '{actionKey}' was found.", targetType.Name, actionKey);
					returnValue = (IDoAction<TInputs, TResult>)doAction;

					//
					// Attempt to dispose the unused items.
					//
					foreach (IDoAction unusedItem in unusedItems)
					{
						this.Logger.LogDebug("Attempting to dispose non-matching item '{item}'.", unusedItem.GetType().Name);
						unusedItem.TryDispose();
					}
				}
				else
				{
					//
					// Dispose the item (if it supports it).
					//
					this.Logger.LogDebug("Attempting to dispose unused item '{item}'.", doAction.GetType().Name);
					doAction.TryDispose();

					this.Logger.LogError("IDecorator of type '{targetType}' and action key of '{actionKey}' was NOT found. Throwing exception...", targetType.Name, actionKey);
					throw new DoActionNotFoundException(typeof(TInputs), typeof(TResult), actionKey);
				}
			}
			else
			{
				this.Logger.LogError("IDecorator of type '{targetType}' and action key of '{actionKey}' was NOT found. Throwing exception...", targetType.Name, actionKey);
				throw new DoActionNotFoundException(typeof(TInputs), typeof(TResult), actionKey);
			}

			return Task.FromResult(returnValue);
		}
	}
}

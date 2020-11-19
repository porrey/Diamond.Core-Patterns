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
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.WorkFlow
{
	/// <summary>
	/// This is a generic repository factory that can return a repository
	/// for any given entity interface.
	/// </summary>
	public class WorkFlowManagerFactory : IWorkFlowManagerFactory
	{
		public WorkFlowManagerFactory(IObjectFactory objectFactory)
		{
			this.ObjectFactory = objectFactory;
		}

		public WorkFlowManagerFactory(IObjectFactory objectFactory, ILoggerSubscriber loggerSubscriber)
		{
			this.ObjectFactory = objectFactory;
			this.LoggerSubscriber = loggerSubscriber;
		}

		public ILoggerSubscriber LoggerSubscriber { get; set; } = new NullLoggerSubscriber();
		protected IObjectFactory ObjectFactory { get; set; }

		public Task<IWorkFlowManager<TContextDecorator, TContext>> GetAsync<TContextDecorator, TContext>(string groupName)
			where TContextDecorator : IContextDecorator<TContext>
			where TContext : IContext
		{
			IWorkFlowManager<TContextDecorator, TContext> returnValue = null;

			// ***
			// *** Get the type being requested.
			// ***
			Type targetType = typeof(IWorkFlowManager<TContextDecorator, TContext>);
			this.LoggerSubscriber.Verbose($"Location Work-Flow manager with group name '{groupName}'.");

			// ***
			// *** Find the repository that supports the given type.
			// ***
			IEnumerable<IWorkFlowManager> items = this.ObjectFactory.GetAllInstances<IWorkFlowManager>();
			IWorkFlowManager item = items.Where(t => t.Group == groupName).SingleOrDefault();

			if (item != null)
			{
				this.LoggerSubscriber.Verbose($"Work-Flow manager with group '{groupName}' was found.");
				returnValue = (IWorkFlowManager<TContextDecorator, TContext>)item;
				this.LoggerSubscriber.AddToInstance(returnValue);
			}
			else
			{
				this.LoggerSubscriber.Warning($"Work-Flow manager with group '{groupName}' was NOT found.");
				throw new WorkFlowManagerNotFoundException<TContextDecorator, TContext>(groupName);
			}

			return Task.FromResult(returnValue);
		}
	}
}

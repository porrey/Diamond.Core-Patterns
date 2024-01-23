//
// Copyright(C) 2019-2024, Daniel M. Porrey. All rights reserved.
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

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// This is a generic repository factory that can return a repository
	/// for any given entity interface.
	/// </summary>
	public class WorkflowManagerFactory : IWorkflowManagerFactory
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		public WorkflowManagerFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="logger"></param>
		public WorkflowManagerFactory(IServiceProvider serviceProvider, ILogger<WorkflowManagerFactory> logger)
			: this(serviceProvider)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual ILogger<WorkflowManagerFactory> Logger { get; set; } = new NullLogger<WorkflowManagerFactory>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		public virtual Task<IWorkflowManager> GetAsync(string groupName)
		{
			IWorkflowManager returnValue = null;

			//
			// Get the type being requested.
			//
			Type targetType = typeof(IWorkflowManager);
			this.Logger.LogDebug("Location workflow manager with group name '{groupName}'.", groupName);

			//
			// Find the repository that supports the given type.
			//
			IEnumerable<IWorkflowManager> items = this.ServiceProvider.GetService<IEnumerable<IWorkflowManager>>();
			IWorkflowManager item = items.Where(t => t.Group == groupName).SingleOrDefault();

			if (item != null)
			{
				this.Logger.LogDebug("Workflow manager with group '{groupName}' was found.", groupName);
				returnValue = (IWorkflowManager)item;
			}
			else
			{
				this.Logger.LogWarning("Workflow manager with group '{groupName}' was NOT found.", groupName);
				throw new WorkflowManagerNotFoundException(groupName);
			}

			return Task.FromResult(returnValue);
		}
	}
}

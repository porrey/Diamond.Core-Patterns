//
// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
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
		/// <param name="serviceKey">The container service key.</param>
		/// <returns></returns>
		public virtual Task<IWorkflowManager> GetAsync(string serviceKey)
		{
			IWorkflowManager returnValue = null;

			//
			// Find the repository that supports the given type.
			//
			this.Logger.LogDebug("Finding workflow manager with Service Key '{serviceKey}''.", serviceKey);
			IWorkflowManager item = this.ServiceProvider.GetKeyedService<IWorkflowManager>(serviceKey);

			if (item != null)
			{
				this.Logger.LogDebug("Workflow manager with Service key '{servicekey}' was found.", serviceKey);
				returnValue = item;
			}
			else
			{
				this.Logger.LogWarning("Workflow manager with Service key '{servicekey}' was NOT found.", serviceKey);
				throw new WorkflowManagerNotFoundException(serviceKey);
			}

			return Task.FromResult(returnValue);
		}
	}
}

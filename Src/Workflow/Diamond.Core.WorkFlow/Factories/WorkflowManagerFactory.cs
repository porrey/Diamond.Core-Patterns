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
		/// Initializes a new instance of the WorkflowManagerFactory class using the specified service provider.
		/// </summary>
		/// <param name="serviceProvider">The service provider used to resolve dependencies required by the factory. Cannot be null.</param>
		public WorkflowManagerFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// Initializes a new instance of the WorkflowManagerFactory class with the specified service provider and logger.
		/// </summary>
		/// <param name="serviceProvider">The service provider used to resolve dependencies required by the factory.</param>
		/// <param name="logger">The logger used to record diagnostic and operational information for the factory.</param>
		public WorkflowManagerFactory(ILogger<WorkflowManagerFactory> logger, IServiceProvider serviceProvider)
			: this(serviceProvider)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Gets or sets the service provider used to resolve service dependencies within the class.
		/// </summary>
		/// <remarks>Assigning a custom service provider allows for advanced dependency injection scenarios, such as
		/// substituting services during testing or extending functionality. The property is typically used by derived classes
		/// to access or override service resolution behavior.</remarks>
		protected virtual IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// Gets or sets the logger used for logging workflow manager factory operations.
		/// </summary>
		/// <remarks>Assign a custom logger to capture diagnostic information or errors during workflow manager
		/// factory execution. By default, logging is disabled unless a logger is provided.</remarks>
		public virtual ILogger<WorkflowManagerFactory> Logger { get; set; } = new NullLogger<WorkflowManagerFactory>();

		/// <summary>
		/// Asynchronously retrieves the workflow manager associated with the specified service key.
		/// </summary>
		/// <remarks>This method searches for a workflow manager registered with the given service key. If no matching
		/// manager is found, an exception is thrown. The operation completes synchronously but returns a task for
		/// compatibility with asynchronous patterns.</remarks>
		/// <param name="serviceKey">The unique key identifying the workflow manager to retrieve. Cannot be null or empty.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the workflow manager instance
		/// associated with the specified service key.</returns>
		/// <exception cref="WorkflowManagerNotFoundException">Thrown if no workflow manager is found for the specified service key.</exception>
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
				this.Logger.LogDebug("Workflow manager with Service key '{serviceKey}' was found.", serviceKey);
				item.ServiceKey = serviceKey;
				returnValue = item;
			}
			else
			{
				this.Logger.LogWarning("Workflow manager with Service key '{serviceKey}' was NOT found.", serviceKey);
				throw new WorkflowManagerNotFoundException(serviceKey);
			}

			return Task.FromResult(returnValue);
		}
	}
}

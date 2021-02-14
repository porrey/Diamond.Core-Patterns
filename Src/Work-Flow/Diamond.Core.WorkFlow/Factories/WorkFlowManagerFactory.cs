//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using Diamond.Core.Extensions.InterfaceInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.WorkFlow {
	/// <summary>
	/// This is a generic repository factory that can return a repository
	/// for any given entity interface.
	/// </summary>
	public class WorkFlowManagerFactory : IWorkFlowManagerFactory, ILoggerPublisher<WorkFlowManagerFactory> {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		public WorkFlowManagerFactory(IServiceProvider serviceProvider) {
			this.ServiceProvider = serviceProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="logger"></param>
		public WorkFlowManagerFactory(IServiceProvider serviceProvider, ILogger<WorkFlowManagerFactory> logger) {
			this.ServiceProvider = serviceProvider;
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		protected IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ILogger<WorkFlowManagerFactory> Logger { get; set; } = new NullLogger<WorkFlowManagerFactory>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		public Task<IWorkFlowManager> GetAsync(string groupName) {
			IWorkFlowManager returnValue = null;

			//
			// Get the type being requested.
			//
			Type targetType = typeof(IWorkFlowManager);
			this.Logger.LogDebug($"Location Work-Flow manager with group name '{groupName}'.");

			//
			// Find the repository that supports the given type.
			//
			IEnumerable<IWorkFlowManager> items = this.ServiceProvider.GetService<IEnumerable<IWorkFlowManager>>();
			IWorkFlowManager item = items.Where(t => t.Group == groupName).SingleOrDefault();

			if (item != null) {
				this.Logger.LogDebug($"Work-Flow manager with group '{groupName}' was found.");
				returnValue = (IWorkFlowManager)item;
			}
			else {
				this.Logger.LogWarning($"Work-Flow manager with group '{groupName}' was NOT found.");
				throw new WorkFlowManagerNotFoundException(groupName);
			}

			return Task.FromResult(returnValue);
		}
	}
}

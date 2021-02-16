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
using Diamond.Core.Workflow;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	public static class WorkflowDependencies
	{
		public static IServiceCollection AddWorkflowExampleDependencies(this IServiceCollection services)
		{
			//
			// Add the sample work flow manager and work flow steps.
			//
			services.AddScoped<IWorkflowManagerFactory, WorkflowManagerFactory>()
					.AddScoped<IWorkflowItemFactory, WorkflowItemFactory>()
					.AddScoped<IWorkflowManager, SampleWorkflowManager>()
					.AddScoped<IWorkflowItem, SampleWorkStep1>()
					.AddScoped<IWorkflowItem, SampleWorkStep2>()
					.AddScoped<IWorkflowItem, SampleWorkStep3>()
					.AddScoped<IWorkflowItem, SampleWorkStep4>()
					.AddScoped<IWorkflowItem, SampleWorkStep5>();

			return services;
		}
	}
}

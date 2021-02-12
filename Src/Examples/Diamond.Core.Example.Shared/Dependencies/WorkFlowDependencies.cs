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
using Diamond.Core.WorkFlow;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	public static class WorkFlowDependencies
	{
		public static IServiceCollection AddWorkFlowExampleDependencies(this IServiceCollection services)
		{
			//
			// Add the sample work flow manager and work flow steps.
			//
			services.AddScoped<IWorkFlowManagerFactory, WorkFlowManagerFactory>()
					.AddScoped<IWorkFlowItemFactory, WorkFlowItemFactory>()
					.AddScoped<IWorkFlowManager, SampleWorkFlowManager>()
					.AddScoped<IWorkFlowItem, SampleWorkStep1>()
					.AddScoped<IWorkFlowItem, SampleWorkStep2>()
					.AddScoped<IWorkFlowItem, SampleWorkStep3>()
					.AddScoped<IWorkFlowItem, SampleWorkStep4>()
					.AddScoped<IWorkFlowItem, SampleWorkStep5>();

			return services;
		}
	}
}

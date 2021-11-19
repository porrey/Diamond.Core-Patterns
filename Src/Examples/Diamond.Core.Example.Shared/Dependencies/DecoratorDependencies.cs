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
using Diamond.Core.Decorator;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Example
{
	public static class DecoratorDependencies
	{
		public static IServiceCollection AddDecoratorExampleDependencies(this IServiceCollection services)
		{
			//
			// Add the rules to validate the shipment model. 
			//
			services.AddScoped<IDecoratorFactory, DecoratorFactory>()
					.AddScoped<IDecorator, BookTransactionDecorator>();

			return services;
		}
	}
}

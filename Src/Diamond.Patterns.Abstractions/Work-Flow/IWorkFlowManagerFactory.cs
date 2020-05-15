// ***
// *** Copyright(C) 2019-2020, Daniel M. Porrey. All rights reserved.
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
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Defines a factory that will retrieve the work flow manager for a given work flow.
	/// </summary>
	public interface IWorkFlowManagerFactory
	{
		/// <summary>
		/// Gets the work flow items for a given work flow identified by groupName;
		/// </summary>
		/// <typeparam name="TContextDecorator">The type of context decorator used by the work flow item.</typeparam>
		/// <typeparam name="TContext">The type of context used by  the work flow item.</typeparam>
		/// <param name="groupName">a name that groups work flow items together.</param>
		/// <returns>Returns the work flow manager for the specified work flow.</returns>
		Task<IWorkFlowManager<TContextDecorator, TContext>> GetAsync<TContextDecorator, TContext>(string groupName)
			where TContext : IContext
			where TContextDecorator : IContextDecorator<TContext>;
	}
}

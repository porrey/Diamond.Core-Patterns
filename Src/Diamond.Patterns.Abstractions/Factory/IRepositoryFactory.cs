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
	public interface IRepositoryFactory
	{
		Task<IRepository<TInterface>> GetAsync<TInterface>() where TInterface : IEntity;
		Task<IReadOnlyRepository<TInterface>> GetReadOnlyAsync<TInterface>() where TInterface : IEntity;
		Task<IQueryableRepository<TInterface>> GetQueryableAsync<TInterface>() where TInterface : IEntity;
		Task<IWritableRepository<TInterface>> GetWritableAsync<TInterface>() where TInterface : IEntity;

		Task<IRepository<TInterface>> GetAsync<TInterface>(string name = null) where TInterface : IEntity;
		Task<IReadOnlyRepository<TInterface>> GetReadOnlyAsync<TInterface>(string name = null) where TInterface : IEntity;
		Task<IQueryableRepository<TInterface>> GetQueryableAsync<TInterface>(string name = null) where TInterface : IEntity;
		Task<IWritableRepository<TInterface>> GetWritableAsync<TInterface>(string name = null) where TInterface : IEntity;
	}
}

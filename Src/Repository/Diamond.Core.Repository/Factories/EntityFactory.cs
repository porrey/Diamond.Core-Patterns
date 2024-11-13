//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Repository
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TInterface"></typeparam>
	/// <typeparam name="TEntity"></typeparam>
	public class EntityFactory<TInterface, TEntity> : IEntityFactory<TInterface>
		where TEntity : TInterface, new()
		where TInterface : IEntity
	{
		/// <summary>
		/// 
		/// </summary>
		public EntityFactory()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		public EntityFactory(ILogger<EntityFactory<TInterface, TEntity>> logger)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual ILogger<EntityFactory<TInterface, TEntity>> Logger { get; set; } = new NullLogger<EntityFactory<TInterface, TEntity>>();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Task<TInterface> CreateAsync()
		{
			TEntity returnValue = new TEntity();

			this.Logger.LogDebug("Model factory is creating instance of model type '{name}'.", typeof(TEntity).Name);

			return Task.FromResult<TInterface>(returnValue);
		}
	}
}

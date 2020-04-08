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
using System;
using System.Data.Entity;

namespace Diamond.Patterns.DatabaseStrategy.MsSql
{
	internal class MsSqlDropCreateDatabaseIfModelChanges<TContext> : DropCreateDatabaseIfModelChanges<TContext> where TContext : DbContext
	{
		protected EventHandler<TContext> SeedCallback = null;

		public MsSqlDropCreateDatabaseIfModelChanges(EventHandler<TContext> seedCallback)
			: base()
		{
			this.SeedCallback = seedCallback;
		}

		protected override void Seed(TContext context)
		{
			this.SeedCallback?.Invoke(this, context);
		}
	}
}

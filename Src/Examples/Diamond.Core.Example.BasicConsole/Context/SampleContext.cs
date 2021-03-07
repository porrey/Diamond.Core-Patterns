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
using Diamond.Core.Extensions.DependencyInjection;
using Diamond.Core.Repository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Example.BasicConsole
{
	public class SampleContext : RepositoryContext<SampleContext>
	{
		public SampleContext()
			: base()
		{
		}

		public SampleContext(DbContextOptions<SampleContext> options)
			: base(options)
		{
		}

		[Dependency]
		protected ILogger<SampleContext> Logger { get; set; } = new NullLogger<SampleContext>();

		public DbSet<EmployeeEntity> Employees { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			this.Logger.LogDebug($"OnModelCreating() called in {nameof(SampleContext)}");

			//
			// Invoice number must be unique.
			//
			modelBuilder.Entity<EmployeeEntity>().HasIndex(p => new { p.LastName, p.FirstName }).IsUnique();
			base.OnModelCreating(modelBuilder);
		}
	}
}

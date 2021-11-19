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
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Rules
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TItem"></typeparam>
	[Obsolete("Use RuleTemplate instead.")]
	public abstract class Rule<TItem> : RuleTemplate<TItem>
	{
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TItem"></typeparam>
	public abstract class RuleTemplate<TItem> : IRule<TItem>
	{
		/// <summary>
		/// 
		/// </summary>
		public RuleTemplate()
		{
			this.Logger = new NullLogger<RuleTemplate<TItem>>();
		}

		/// <summary>
		/// 
		/// </summary>
		public RuleTemplate(ILogger<RuleTemplate<TItem>> logger)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="group"></param>
		public RuleTemplate(string group)
		{
			this.Logger = new NullLogger<RuleTemplate<TItem>>();
			this.Group = group;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="group"></param>
		public RuleTemplate(ILogger<RuleTemplate<TItem>> logger, string group)
		{
			this.Logger = logger;
			this.Group = group;
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<RuleTemplate<TItem>> Logger { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string Group { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<IRuleResult> ValidateAsync(TItem item)
		{
			return this.OnValidateAsync(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected virtual Task<IRuleResult> OnValidateAsync(TItem item)
		{
			throw new NotImplementedException();
		}
	}
}

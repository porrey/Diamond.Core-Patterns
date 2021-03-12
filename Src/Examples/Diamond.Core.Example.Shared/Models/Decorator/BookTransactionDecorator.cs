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
using System.Threading.Tasks;
using Diamond.Core.Decorator;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	public class BookTransactionDecorator : DecoratorTemplate<IBook, bool>
	{
		public BookTransactionDecorator(ILogger<BookTransactionDecorator> logger)
			: base(logger)
		{
		}

		public override string Name => WellKnown.Decorator.BookTransaction;

		protected override Task<bool> OnTakeActionAsync(IBook item)
		{
			bool returnValue = false;

			if (item.CheckedOut)
			{
				this.Logger.LogWarning($"The book '{item.Title}' is already checked out.");
				returnValue = false;
			}
			else
			{
				item.CheckedOut = true;
				returnValue = true;
				this.Logger.LogWarning($"The book '{item.Title}' was successfully checked out.");
			}

			return Task.FromResult(returnValue);
		}
	}
}

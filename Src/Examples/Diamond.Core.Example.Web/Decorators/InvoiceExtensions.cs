using System.Collections.Generic;
using System.Linq;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public static class InvoiceExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static InvoiceItem FromEntity(this IInvoice item)
		{
			return new InvoiceItem()
			{
				Id = item.Id,
				Number = item.Number,
				Description = item.Description,
				Total = item.Total
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		public static IInvoice CopyTo(this InvoiceItem item, IInvoice model)
		{
			model.Id = item.Id;
			model.Number = item.Number;
			model.Description = item.Description;
			model.Total = item.Total;
			return model;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		public static IInvoice Update(this IInvoice model, InvoiceItem item)
		{
			model.Description = item.Description;
			model.Total = item.Total;
			return model;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public static IEnumerable<InvoiceItem> ToInvoiceItemList(this IEnumerable<IInvoice> query)
		{
			return from tbl in query
					select new InvoiceItem()
					{
						Id = tbl.Id,
						Number = tbl.Number,
						Description = tbl.Description,
						Total = tbl.Total
					};
		}
	}
}

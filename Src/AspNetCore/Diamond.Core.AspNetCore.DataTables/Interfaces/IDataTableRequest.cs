using Microsoft.AspNetCore.Http;

namespace Diamond.Core.AspNetCore.DataTables
{
	public interface IDataTableRequest
	{
		Column[] Columns { get; set; }
		int Draw { get; set; }
		int Length { get; set; }
		Order[] Order { get; set; }
		Search Search { get; set; }
		FormCollection SearchBuilder { get; set; }
		int Start { get; set; }
	}
}
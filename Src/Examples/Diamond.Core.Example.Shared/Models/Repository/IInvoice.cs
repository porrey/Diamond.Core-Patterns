using Diamond.Core.Repository;

namespace Diamond.Core.Example
{
	public interface IInvoice : IEntity<int>
	{
		string Description { get; set; }
		string Number { get; set; }
		float Total { get; set; }
	}
}
namespace Diamond.Patterns.Example
{
	public interface IShipmentModel
	{
		string ProNumber { get; set; }
		string DeliveryAddress { get; set; }
		int PalletCount { get; set; }
		string PickupAddress { get; set; }
		float Weight { get; set; }
	}
}
namespace Diamond.Core.Example
{
	public class ShipmentModel : IShipmentModel
	{
		public string ProNumber { get; set; }
		public string PickupAddress { get; set; }
		public string DeliveryAddress { get; set; }
		public float Weight { get; set; }
		public int PalletCount { get; set; }
	}
}

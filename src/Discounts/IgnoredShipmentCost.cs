namespace Discounts
{
    public class IgnoredShipmentCost : IShipmentCost<IgnoredShipment>
    {
        public IgnoredShipmentCost(IgnoredShipment shipment)
        {
            Shipment = shipment;
        }

        public IgnoredShipment Shipment { get; }
        public string Format(string separator)
        {
            return Shipment.Format() + separator + "Ignored";
        }

        public IShipmentCost<IgnoredShipment> MinimizeCostForSize(decimal minCost ,string targetSize)
        {
            return this;
        }
    }
}
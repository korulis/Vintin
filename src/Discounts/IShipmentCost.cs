namespace Discounts
{
    public interface IShipmentCost<out TShipment> where TShipment : IShipment
    {
        TShipment Shipment { get; }
        string Format(string separator);
        IShipmentCost<TShipment> MinimizeCostForSize(decimal minCost, string targetSize);
    }
}
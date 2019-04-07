using System;
using System.Collections.Generic;
using System.IO;
using Discounts;

namespace ConsoleApp
{
    public class FileBasedShipmentProcessor
    {
        private readonly ShipmentMapper _shipmentMapper;
        private readonly ShipmentCostCalculator _shipmentCostCalculator;
        private readonly Action<IEnumerable<string>> _outputMethod;

        public FileBasedShipmentProcessor(
            ShipmentMapper shipmentMapper,
            ShipmentCostCalculator shipmentCostCalculator,
            Action<IEnumerable<string>> outputMethod)
        {
            _shipmentMapper = shipmentMapper;
            _shipmentCostCalculator = shipmentCostCalculator;
            _outputMethod = outputMethod;
        }

        public void Process(string inputFilePath)
        {
            var shipmentLines = File.ReadLines(inputFilePath);

            var shipments = _shipmentMapper.ParseInput(shipmentLines);

            var pricedShipments = _shipmentCostCalculator.CalculatePrice(shipments);

            var pricedShipmentLines = _shipmentMapper.FormatOutput(pricedShipments);

            _outputMethod(pricedShipmentLines);
        }
    }
}
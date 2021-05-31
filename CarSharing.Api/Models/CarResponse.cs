using CarSharing.Core.Enums;
using CarSharing.Core.ValueObjects;

namespace CarSharing.Api.Models
{
    public class CarResponse
    {
        public string Id { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public string Plate { get; set; }

        public decimal RangeRemainingKm { get; set; }

        public decimal FuelRemainingFraction { get; set; }

        public TransmissionType Transmission { get; set; }

        public FuelType Fuel { get; set; }

        public string Address { get; set; }

        public LatLng Position { get; set; }
    }
}

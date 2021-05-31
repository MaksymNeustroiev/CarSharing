using CarSharing.Core.Enums;

namespace CarSharing.Core.ValueObjects
{
    public class Engine
    {
        public FuelType Fuel { get; set; }

        public TransmissionType Transmission { get; set; }
    }
}

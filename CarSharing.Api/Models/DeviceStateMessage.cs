using CarSharing.Application.UseCases.DeviceState;

namespace CarSharing.Api.Models
{
    public class DeviceStateMessage
    {
        public string Imei { get; set; }

        public int MileageTotalKm { get; set; }

        public decimal FuelRemainingFraction { get; set; }

        public bool Ignition { get; set; }

        public bool Locked { get; set; }

        public LatLng Position { get; set; }
    }
}

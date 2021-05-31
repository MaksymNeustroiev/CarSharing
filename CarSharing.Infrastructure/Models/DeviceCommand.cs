namespace CarSharing.Infrastructure.Models
{
    public sealed class DeviceCommand
    {
        public string Imei { get; set; }

        public CommandType Command { get; set; }
    }
}

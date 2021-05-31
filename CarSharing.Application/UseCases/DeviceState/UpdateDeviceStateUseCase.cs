using CarSharing.Core.Repositories;
using System.Threading.Tasks;

namespace CarSharing.Application.UseCases.DeviceState
{
    public class UpdateDeviceStateUseCase : IUpdateDeviceStateUseCase
    {
        private readonly IDeviceStateRepository deviceStateRepository;

        public UpdateDeviceStateUseCase(IDeviceStateRepository deviceStateRepository)
        {
            this.deviceStateRepository = deviceStateRepository;
        }

        public Task<UpdateDeviceStateOutput> HandleAsync(UpdateDeviceStateInput input)
        {
            var output = new UpdateDeviceStateOutput();
            var deviceState = new Core.Entities.DeviceState
            {
                Imei = input.Imei,
                FuelRemainingFraction = input.FuelRemainingFraction,
                Ignition = input.Ignition,
                Locked = input.Locked,
                MileageTotalKm = input.MileageTotalKm,
                Position = new Core.ValueObjects.LatLng
                {
                    Lat = input.Position.Lat,
                    Lng = input.Position.Lng
                }
            };

            deviceStateRepository.Add(deviceState);

            return Task.FromResult(output);
        }
    }
}

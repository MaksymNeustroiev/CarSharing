using CarSharing.Core.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace CarSharing.Application.UseCases.GetCars
{
    public class GetCarsUseCase : IGetCarsUseCase
    {
        private readonly ICarsRepository carsRepository;
        private readonly IDeviceStateRepository deviceStateRepository;

        public GetCarsUseCase(ICarsRepository carsRepository, IDeviceStateRepository deviceStateRepository)
        {
            this.carsRepository = carsRepository;
            this.deviceStateRepository = deviceStateRepository;
        }

        public Task<GetCarsOutput> HandleAsync(GetCarsInput input)
        {
            var cars = carsRepository.GetCars()
                .Where(x => x.Locked == false)
                .Select(x => new
                {
                    GetCarOutput = new GetCarOutput
                    {
                        Id = x.Id,
                        Address = null,
                        Color = x.Color,
                        Model = x.Model,
                        Fuel = x.Engine.Fuel,
                        Plate = x.Plate,
                        Transmission = x.Engine.Transmission,

                        Position = new Core.ValueObjects.LatLng(),
                        RangeRemainingKm = x.RangeFullKm,
                        FuelRemainingFraction = 0
                    },
                    x.Tracker.Imei
                }).ToList();

            foreach (var car in cars)
            {
                var deviceState = deviceStateRepository.GetLastState(car.Imei);

                if (deviceState != null)
                {
                    car.GetCarOutput.Position = deviceState.Position;
                    car.GetCarOutput.FuelRemainingFraction = deviceState.FuelRemainingFraction;
                }

            }

            var output = new GetCarsOutput { Cars = cars.Select(x => x.GetCarOutput).ToList() };
            return Task.FromResult(output);
        }
    }

}

using CarSharing.Core.Entities;
using CarSharing.Core.Enums;
using CarSharing.Core.Repositories;
using CarSharing.Core.Services;
using System.Threading.Tasks;

namespace CarSharing.Application.UseCases.StartBooking
{
    public class StartBookingUseCase : IStartBookingUseCase
    {
        private readonly ICarsRepository carsRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly ICarLockingService carLockingService;

        public StartBookingUseCase(ICarsRepository carsRepository, IBookingRepository bookingRepository, ICarLockingService carLockingService)
        {
            this.carsRepository = carsRepository;
            this.bookingRepository = bookingRepository;
            this.carLockingService = carLockingService;
        }

        public async Task<StartBookingOutput> HandleAsync(StartBookingInput input)
        {
            var output = new StartBookingOutput();
            var car = carsRepository.GetById(input.CarId);
            if (car == null)
            {
                output.Errors.Add(Error.CarNotFound);
                return output;
            }

            var lockSuccessfull = carsRepository.Lock(car);
            if (!lockSuccessfull)
            {
                output.Errors.Add(Error.CarNotAvailable);
                return output;
            }

            var booking = new Booking(car.Id);
            bookingRepository.AddBooking(booking);

            output.BookingId = booking.Id;

            await carLockingService.LockCar(car);

            return output;
        }
    }
}

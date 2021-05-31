using CarSharing.Core.Enums;
using CarSharing.Core.Repositories;
using CarSharing.Core.Services;
using System.Threading.Tasks;

namespace CarSharing.Application.UseCases.FinishBooking
{
    public class FinishBookingUseCase : IFinishBookingUseCase
    {
        private readonly ICarsRepository carsRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly ICarLockingService carLockingService;

        public FinishBookingUseCase(ICarsRepository carsRepository, IBookingRepository bookingRepository, ICarLockingService carLockingService)
        {
            this.carsRepository = carsRepository;
            this.bookingRepository = bookingRepository;
            this.carLockingService = carLockingService;
        }

        public async Task<FinishBookingOutput> HandleAsync(FinishBookingInput input)
        {
            var output = new FinishBookingOutput();
            var booking = bookingRepository.GetBooking(input.BookingId);

            if (booking == null)
            {
                output.Errors.Add(Error.BookingNotFound);
                return output;
            }

            var car = carsRepository.GetById(booking.CarId);
            if (booking == null)
            {
                output.Errors.Add(Error.CarNotFound);
                return output;
            }

            var finishBookingSuccessfull = bookingRepository.FinishBooking(booking);

            if (!finishBookingSuccessfull)
            {
                output.Errors.Add(Error.BookingNotActive);
                return output;
            }

            carsRepository.Unlock(car);
            await carLockingService.UnlockCar(car);

            return output;
        }
    }
}

using CarSharing.Application.UseCases.StartBooking;
using CarSharing.Core.Entities;
using CarSharing.Core.Repositories;
using CarSharing.Core.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CarSharing.Tests.Application
{
    public class StartBookingUseCaseTests
    {
        private readonly StartBookingUseCase startBookingUseCase;
        private readonly Mock<ICarsRepository> carsRepositoryMock;
        private readonly Mock<IBookingRepository> bookingRepositoryMock;
        private readonly Mock<ICarLockingService> carLockingServiceMock;


        private readonly string carId = Guid.NewGuid().ToString();

        public StartBookingUseCaseTests()
        {
            carsRepositoryMock = new Mock<ICarsRepository>();
            bookingRepositoryMock = new Mock<IBookingRepository>();
            carLockingServiceMock = new Mock<ICarLockingService>();

            startBookingUseCase = new StartBookingUseCase(carsRepositoryMock.Object,
                bookingRepositoryMock.Object,
                carLockingServiceMock.Object);
        }

        [Fact]
        public async Task ShouldCall_GetCar()
        {
            //Arrange
            var input = new StartBookingInput { CarId = carId };

            //Act
            var result = await startBookingUseCase.HandleAsync(input);

            //Assert
            carsRepositoryMock.Verify(x => x.GetById(carId), Times.Once);
        }

        [Fact]
        public async Task ShouldCall_LockCar()
        {
            //Arrange
            var input = new StartBookingInput { CarId = carId };
            var car = new Car { Id = carId };
            carsRepositoryMock
                .Setup(x => x.GetById(carId))
                .Returns(car);

            //Act
            var result = await startBookingUseCase.HandleAsync(input);

            //Assert
            carsRepositoryMock.Verify(x => x.Lock(car), Times.Once);
        }

    }
}

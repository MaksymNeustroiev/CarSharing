using CarSharing.Core.Entities;
using CarSharing.Infrastructure.DataAccess;
using Xunit;

namespace CarSharing.Tests.Repositories
{
    public class CarsRepositoryTests
    {
        private readonly CarsRepository carsRepository;

        public CarsRepositoryTests()
        {
            carsRepository = new CarsRepository();
        }

        [Fact]
        public void Lock_ShouldSetLockedTrue()
        {
            //Arrange
            var car = new Car();

            //Act
            carsRepository.Lock(car);

            //Assert
            Assert.True(car.Locked);
        }

        [Fact]
        public void Unlock_ShouldSetLockedFalse()
        {
            //Arrange
            var car = new Car { Locked = true };

            //Act
            carsRepository.Unlock(car);

            //Assert
            Assert.False(car.Locked);
        }


    }
}

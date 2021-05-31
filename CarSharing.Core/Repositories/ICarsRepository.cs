using CarSharing.Core.Entities;
using System.Collections.Generic;

namespace CarSharing.Core.Repositories
{
    public interface ICarsRepository
    {
        Car GetById(string carId);
        IEnumerable<Car> GetCars();
        bool Lock(Car car);
        bool Unlock(Car car);
    }
}

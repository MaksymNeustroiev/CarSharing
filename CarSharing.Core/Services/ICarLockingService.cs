using CarSharing.Core.Entities;
using System.Threading.Tasks;

namespace CarSharing.Core.Services
{
    public interface ICarLockingService
    {
        Task LockCar(Car car);
        Task UnlockCar(Car car);
    }
}

using CarSharing.Core.Entities;

namespace CarSharing.Core.Repositories
{
    public interface IDeviceStateRepository
    {
        void Add(DeviceState deviceState);
        DeviceState GetLastState(string imei);
    }
}

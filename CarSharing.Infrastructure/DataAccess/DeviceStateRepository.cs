using CarSharing.Core.Entities;
using CarSharing.Core.Repositories;
using System.Collections.Generic;

namespace CarSharing.Infrastructure.DataAccess
{
    public class DeviceStateRepository : IDeviceStateRepository
    {
        private static readonly Dictionary<string, DeviceState> data = new Dictionary<string, DeviceState>();

        public void Add(DeviceState deviceState)
        {
            data[deviceState.Imei] = deviceState;
        }

        public DeviceState GetLastState(string imei)
        {
            if (data.ContainsKey(imei))
            {
                return data[imei];
            }
            return null;
        }
    }
}

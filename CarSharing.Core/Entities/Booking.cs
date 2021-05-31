using System;

namespace CarSharing.Core.Entities
{
    public class Booking
    {
        public string Id { get; private set; }
        public string CarId { get; private set; }
        public bool IsActive { get; set; }

        public Booking(string carId)
        {
            Id = Guid.NewGuid().ToString();
            CarId = carId;
            IsActive = true;
        }
    }
}

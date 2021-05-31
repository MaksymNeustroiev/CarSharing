using CarSharing.Core.Entities;
using CarSharing.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace CarSharing.Infrastructure.DataAccess
{
    public class BookingRepository : IBookingRepository
    {
        private static readonly object lockObject = new object();
        private static readonly List<Booking> bookings = new List<Booking>();

        public void AddBooking(Booking booking)
        {
            bookings.Add(booking);
        }

        public bool FinishBooking(Booking booking)
        {
            lock (lockObject)
            {
                if (!booking.IsActive)
                {
                    return false;
                }

                booking.IsActive = false;
                return true;
            }
        }

        public Booking GetBooking(string id)
        {
            return bookings.FirstOrDefault(x => x.Id == id);
        }
    }
}

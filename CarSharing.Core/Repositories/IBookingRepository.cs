using CarSharing.Core.Entities;

namespace CarSharing.Core.Repositories
{
    public interface IBookingRepository
    {
        void AddBooking(Booking booking);
        bool FinishBooking(Booking booking);
        Booking GetBooking(string id);
    }
}

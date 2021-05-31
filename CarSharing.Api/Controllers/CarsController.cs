using CarSharing.Api.Models;
using CarSharing.Application.UseCases.FinishBooking;
using CarSharing.Application.UseCases.GetCars;
using CarSharing.Application.UseCases.StartBooking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarSharing.Api.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IStartBookingUseCase startBookingUseCase;
        private readonly IFinishBookingUseCase finishBookingUseCase;
        private readonly IGetCarsUseCase getCarsUseCase;

        public CarsController(
            IStartBookingUseCase startBookingUseCase,
            IFinishBookingUseCase finishBookingUseCase,
            IGetCarsUseCase getCarsUseCase)
        {
            this.startBookingUseCase = startBookingUseCase;
            this.finishBookingUseCase = finishBookingUseCase;
            this.getCarsUseCase = getCarsUseCase;
        }

        [HttpPost("bookings/start")]
        [ProducesResponseType(typeof(BookCarResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> StartBooking([FromBody] BookCarRequest bookCarRequest)
        {
            var result = await startBookingUseCase.HandleAsync(new StartBookingInput
            {
                CarId = bookCarRequest.CarId
            });

            if (result.Errors.Count > 0)
            {
                return Conflict();
            }

            var response = new BookCarResponse { BookingId = result.BookingId };
            return Ok(response);
        }

        [HttpPost("bookings/finish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> FinishBooking([FromBody] FinishBookingRequest finishBookingRequest)
        {
            var result = await finishBookingUseCase.HandleAsync(new FinishBookingInput
            {
                BookingId = finishBookingRequest.BookingId
            });

            if (result.Errors.Count > 0)
            {
                throw new ApplicationException(result.Errors.First().ToString());
            }

            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetCarsResponse), StatusCodes.Status200OK)]
        public async Task<GetCarsResponse> GetCars()
        {
            var cars = await getCarsUseCase.HandleAsync(new GetCarsInput());

            var response = new GetCarsResponse
            {
                Cars = cars.Cars.Select(x => new CarResponse
                {
                    Id = x.Id,
                    Address = x.Address,
                    Color = x.Color,
                    Model = x.Model,
                    Fuel = x.Fuel,
                    Plate = x.Plate,
                    Transmission = x.Transmission,
                    Position = new Core.ValueObjects.LatLng { Lat = x.Position.Lat, Lng = x.Position.Lng },
                    RangeRemainingKm = x.RangeRemainingKm,
                    FuelRemainingFraction = x.FuelRemainingFraction
                }).ToList()
            };

            return response;
        }

    }
}

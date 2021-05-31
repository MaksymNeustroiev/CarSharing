using System.Collections.Generic;

namespace CarSharing.Api.Models
{
    public sealed class GetCarsResponse
    {
        public IEnumerable<CarResponse> Cars { get; set; }
    }
}

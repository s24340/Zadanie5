using Microsoft.AspNetCore.Mvc;
using rest.Models.DTO;
using rest.Repositories;

namespace rest.Controllers{

    [Route("api/trip")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly TripRepository.ITripDbRepository repository;

        public TripController(TripRepository.ITripDbRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await repository.GetTripsAsync();
            return Ok(trips);
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddTripToClient([FromRoute] int idTrip, [FromBody] AddTrip dto)
        {
            await repository.AddTripToClientAsync(idTrip, dto);
            return Ok("Your request processed successfully!");
        }
    }
}
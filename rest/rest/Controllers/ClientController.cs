using Microsoft.AspNetCore.Mvc;
using rest.Repositories;

namespace rest.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientRepository.IClientDbRepository repository;

        public ClientController(ClientRepository.IClientDbRepository repository)
        {
            this.repository = repository;
        }

        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient([FromRoute] int idClient)
        {
            try
            {
                await repository.DeleteClientAsync(idClient);
                return Ok($"Client ID: {idClient} was deleted!");
            } 
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}


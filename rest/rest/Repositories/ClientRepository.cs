using Microsoft.EntityFrameworkCore;
using rest.Models;
namespace rest.Repositories;
public class ClientRepository
{
    public interface IClientDbRepository
    {
        Task DeleteClientAsync(int idClient);
    }

    public class ClientDbRepository : IClientDbRepository
    {
        private readonly DatabaseContext context;

        public ClientDbRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public async Task DeleteClientAsync(int idClient)
        {
            bool hasTrips = await context.ClientTrips.AnyAsync(row => row.IdClient == idClient);

            if (hasTrips) throw new Exception("Client has one or more trips!");

            Client client = await context.Clients.Where(row => row.IdClient == idClient).FirstOrDefaultAsync();
            context.Remove(client);

            await context.SaveChangesAsync();
        }
    }
}


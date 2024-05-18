using rest.Models;
using rest.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace rest.Repositories;

public class TripRepository
{
        public interface ITripDbRepository
        {
            Task<IEnumerable<GetTrips>> GetTripsAsync();
            Task AddTripToClientAsync(int idTrip, AddTrip dto);
        }
    
        public class TripDbRepository : ITripDbRepository
        {
            private readonly DatabaseContext _context;
    
            public TripDbRepository(DatabaseContext context)
            {
                _context = context;
            }
    
            public async Task AddTripToClientAsync(int idTrip, AddTrip dto)
            {
                var client = await _context.Clients.FirstOrDefaultAsync(row => row.Pesel == dto.Pesel);
                if (client == null)
                {
                    client = new Client
                    {
                        IdClient = await _context.Clients.Select(row => row.IdClient).MaxAsync() + 1,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Email = dto.Email,
                        Telephone = dto.Telephone,
                        Pesel = dto.Pesel
                    };
    
                    await _context.Clients.AddAsync(client);
                    await _context.SaveChangesAsync();
                }
    
                if (!await _context.Trips.AnyAsync(row => row.IdTrip == idTrip))
                    throw new Exception($"There is no such trip with ID: {idTrip}!");
    
                if (await _context.ClientTrips.AnyAsync(row => row.IdClient == client.IdClient && row.IdTrip == idTrip))
                    throw new Exception("Client is already reserved for that trip!");
    
                await _context.ClientTrips.AddAsync(new ClientTrip
                {
                    IdClient = client.IdClient,
                    IdTrip = idTrip,
                    RegisteredAt = DateTime.Now,
                    PaymentDate = dto.PaymentDate
                });
    
                await _context.SaveChangesAsync();
            }
    
            public async Task<IEnumerable<GetTrips>> GetTripsAsync()
            {
                var trips = await _context.Trips.Select(row => new GetTrips
                {
                    Name = row.Name,
                    Description = row.Description,
                    DateFrom = row.DateFrom,
                    DateTo = row.DateTo,
                    MaxPeople = row.MaxPeople,
                    Countries = row.CountryTrips.Select(ct => new CountryResponse { Name = ct.IdCountryNavigation.Name }),
                    Clients = row.ClientTrips.Select(ct => new ClientResponse
                    {
                        FirstName = ct.IdClientNavigation.FirstName,
                        LastName = ct.IdClientNavigation.LastName
                    })
                }).OrderByDescending(row => row.DateFrom).ToListAsync();
    
                if (!trips.Any())
                    throw new Exception("There is no data in collection!");
    
                return trips;
            }
        }
}






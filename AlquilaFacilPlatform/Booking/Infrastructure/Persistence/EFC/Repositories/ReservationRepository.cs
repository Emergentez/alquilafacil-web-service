using AlquilaFacilPlatform.Booking.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Booking.Domain.Repositories;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Configuration;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AlquilaFacilPlatform.Booking.Infrastructure.Persistence.EFC.Repositories;

public class ReservationRepository(AppDbContext context) : BaseRepository<Reservation>(context), IReservationRepository
{
    public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId)
    {
        return await Context.Set<Reservation>().Where(r => r.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetReservationByStartDateAsync(DateTime startDate)
    {
        return await Context.Set<Reservation>().Where(r => r.StartDate == startDate).ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetReservationByEndDateAsync(DateTime endDate)
    {
           return await Context.Set<Reservation>().Where(r => r.EndDate == endDate).ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByLocalIdsListAsync(List<int> localIdsList)
    {
        return await Context.Set<Reservation>().Where(r => localIdsList.Contains(r.LocalId)).ToListAsync();
    }
}
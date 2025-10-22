using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Domain.Repositories;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Configuration;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AlquilaFacilPlatform.Management.Infrastructure.Persistence.EFC.Repositories;

public class ReadingRepository(AppDbContext context) : BaseRepository<Reading>(context), IReadingRepository
{
    public async Task<IEnumerable<Reading>> FindAllByLocalId(int localId)
    {
        return await Context.Set<Reading>()
            .Where(reading => reading.LocalId == localId)
            .OrderByDescending(reading => reading.Id)
            .ToListAsync();
    }
}
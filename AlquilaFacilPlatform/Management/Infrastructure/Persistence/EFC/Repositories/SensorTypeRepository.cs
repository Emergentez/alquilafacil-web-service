using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.Management.Domain.Repositories;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Configuration;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AlquilaFacilPlatform.Management.Infrastructure.Persistence.EFC.Repositories;

public class SensorTypeRepository(AppDbContext dbContext): BaseRepository<SensorType>(dbContext), ISensorTypeRepository
{
    public Task<bool> ExistsSensorType(ESensorTypes type)
    {
        return Context.Set<SensorType>().AnyAsync(x => x.Type == type.ToString());
    }
}
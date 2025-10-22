using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.Management.Domain.Repositories;

public interface IReadingRepository: IBaseRepository<Reading>
{
    Task<IEnumerable<Reading>> FindAllByLocalId(int localId);
}
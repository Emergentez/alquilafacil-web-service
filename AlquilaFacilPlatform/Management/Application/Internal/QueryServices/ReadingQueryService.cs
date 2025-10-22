using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Domain.Model.Queries;
using AlquilaFacilPlatform.Management.Domain.Repositories;
using AlquilaFacilPlatform.Management.Domain.Services;

namespace AlquilaFacilPlatform.Management.Application.Internal.QueryServices;

public class ReadingQueryService(IReadingRepository readingRepository): IReadingQueryService
{

    public async Task<IEnumerable<Reading>> Handle(GetAllReadingsByLocalIdQuery query)
    {
        return await readingRepository.FindAllByLocalId(query.LocalId);
    }
}
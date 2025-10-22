using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Domain.Model.Queries;

namespace AlquilaFacilPlatform.Management.Domain.Services;

public interface IReadingQueryService
{
    Task<IEnumerable<Reading>> Handle(GetAllReadingsByLocalIdQuery query);
}
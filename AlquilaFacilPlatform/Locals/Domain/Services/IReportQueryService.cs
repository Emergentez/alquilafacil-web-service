using System.Reflection.Metadata;
using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;

namespace AlquilaFacilPlatform.Locals.Domain.Services;

public interface IReportQueryService
{
    Task<IEnumerable<Report?>> Handle(GetReportsByLocalIdQuery query);
    Task<IEnumerable<Report?>> Handle(GetReportsByUserIdQuery query);
}
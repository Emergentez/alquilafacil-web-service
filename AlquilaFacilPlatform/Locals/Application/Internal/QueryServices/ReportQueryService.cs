using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;
using AlquilaFacilPlatform.Locals.Domain.Repositories;
using AlquilaFacilPlatform.Locals.Domain.Services;

namespace AlquilaFacilPlatform.Locals.Application.Internal.QueryServices;

public class ReportQueryService (IReportRepository reportRepository) : IReportQueryService
{
    public async Task<IEnumerable<Report?>> Handle(GetReportsByLocalIdQuery query)
    {
        return await reportRepository.GetReportsByLocalId(query.LocalId);
    }

    public async Task<IEnumerable<Report?>> Handle(GetReportsByUserIdQuery query)
    {
        return await reportRepository.GetReportsByUserId(query.UserId);
    }
}
using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.Locals.Domain.Repositories;

public interface IReportRepository : IBaseRepository<Report>
{
    Task<IEnumerable<Report>> GetReportsByLocalId(int localId);
    Task<IEnumerable<Report>> GetReportsByUserId(int userId);
}
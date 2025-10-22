using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;

namespace AlquilaFacilPlatform.Locals.Domain.Services;

public interface ILocalQueryService
{
    Task<IEnumerable<Local>> Handle(GetAllLocalsQuery query);
    Task<Local?> Handle(GetLocalByIdQuery query);
    Task<HashSet<string>> Handle(GetAllLocalDistrictsQuery query);
    
    Task<IEnumerable<Local>> Handle(GetLocalsByUserIdQuery query);
    Task<IEnumerable<Local>> Handle(GetLocalsByCategoryIdAndCapacityRangeQuery query);
    Task<bool> Handle(IsLocalOwnerQuery query);
    Task<int> Handle(GetLocalOwnerIdByLocalId query);


}
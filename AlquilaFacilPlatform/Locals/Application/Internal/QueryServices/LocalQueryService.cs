using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;
using AlquilaFacilPlatform.Locals.Domain.Repositories;
using AlquilaFacilPlatform.Locals.Domain.Services;

namespace AlquilaFacilPlatform.Locals.Application.Internal.QueryServices;

public class LocalQueryService(ILocalRepository localRepository) : ILocalQueryService
{
    public async Task<IEnumerable<Local>> Handle(GetAllLocalsQuery query)
    {
        return await localRepository.GetLocalsAsync();
    }

    public async Task<Local?> Handle(GetLocalByIdQuery query)
    {
        return await localRepository.GetLocalByIdAsync(query.LocalId);
    }

    public async Task<HashSet<string>> Handle(GetAllLocalDistrictsQuery query)
    {
        return await localRepository.GetAllDistrictsAsync();
    }

    public async Task<IEnumerable<Local>> Handle(GetLocalsByUserIdQuery query)
    {
        return await localRepository.GetLocalsByUserIdAsync(query.UserId);
    }

    public async Task<IEnumerable<Local>> Handle(GetLocalsByCategoryIdAndCapacityRangeQuery query)
    {
        return await localRepository.GetLocalsByCategoryIdAndCapacityRange(query.LocalCategoryId, query.MinCapacity, query.MaxCapacity);
    }

    public async Task<bool> Handle(IsLocalOwnerQuery query)
    {
        return await localRepository.IsOwnerAsync(query.UserId, query.LocalId);
    }
    
    public async Task<int> Handle(GetLocalOwnerIdByLocalId query)
    {
        var ownerId = await localRepository.GetLocalOwnerIdByLocalId(query.LocalId);
        if (ownerId == null)
        {
            throw new Exception("Owner not found");
        }
        return (int)ownerId;
    }
}
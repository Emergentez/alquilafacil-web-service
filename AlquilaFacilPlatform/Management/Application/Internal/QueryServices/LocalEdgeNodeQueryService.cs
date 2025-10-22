using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Domain.Model.Queries;
using AlquilaFacilPlatform.Management.Domain.Repositories;
using AlquilaFacilPlatform.Management.Domain.Services;

namespace AlquilaFacilPlatform.Management.Application.Internal.QueryServices;

public class LocalEdgeNodeQueryService(ILocalEdgeNodeRepository localEdgeNodeRepository): ILocalEdgeNodeQueryService
{
    public async Task<LocalEdgeNode?> Handle(GetLocalEdgeNodeByLocalIdQuery query)
    {
        return await localEdgeNodeRepository.GetByLocalIdAsync(query.LocalId);
    }
}
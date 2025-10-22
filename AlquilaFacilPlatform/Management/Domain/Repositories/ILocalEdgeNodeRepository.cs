using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.Management.Domain.Repositories;

public interface ILocalEdgeNodeRepository: IBaseRepository<LocalEdgeNode>
{
    Task<LocalEdgeNode?> GetByLocalIdAsync(int localId);
}